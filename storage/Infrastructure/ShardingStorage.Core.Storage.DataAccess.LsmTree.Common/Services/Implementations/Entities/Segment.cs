using System.Text;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Models;
using ShardingStorage.Core.Storage.Domain.Entities;
using ShardingStorage.Core.Storage.Domain.Models;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Services.Implementations.Entities;

public class Segment
{
    private readonly ICompressor _compressor;
    private readonly string _filename;
    private readonly string _headerFilename;
    private List<(string, long)>? _blocks;

    private Segment(string filename, string headerFilename, List<(string, long)> blocks, SegmentHeader header,
        ICompressor compressor)
    {
        _filename = filename;
        _blocks = blocks;
        Header = header;
        _compressor = compressor;
        _headerFilename = headerFilename;
    }

    public Segment(string name, ICompressorFactory factory)
    {
        _filename = Filename(name);
        _headerFilename = HeaderFilename(name);
        Header = SegmentHeader.ReadFromFile(_headerFilename);

        _compressor = factory.GetCompressor(Header.CompressorName);
    }

    public SegmentHeader Header { get; }

    public SegmentReader SegmentReader()
    {
        _blocks ??= ReadBlocks();
        return new SegmentReader(_filename, Header, _compressor, _blocks);
    }

    public void Delete()
    {
        File.Delete(_filename);
        File.Delete(_headerFilename);
    }

    public async Task<string?> Get(Key key, CancellationToken token)
    {
        _blocks ??= ReadBlocks();

        var position = LowerBound(_blocks, key);
        if (position is -1)
            return null;

        await using var stream = new FileStream(_filename, FileMode.Open, FileAccess.Read, FileShare.Read);
        var offset = _blocks[position].Item2;
        var maxOffset = position + 1 == _blocks.Count ? stream.Length : _blocks[position + 1].Item2;
        var totalLength = (int)(maxOffset - offset);

        stream.Seek(offset, SeekOrigin.Begin);
        var buffer = new byte[totalLength];
        var _ = await stream.ReadAsync(buffer.AsMemory(0, totalLength), token);

        var memoryStream = new MemoryStream(buffer);
        var decompressedStream = await _compressor.Decompress(memoryStream, token);
        await memoryStream.DisposeAsync();

        using var reader = new BinaryReader(decompressedStream, Encoding.UTF8, false);
        while (reader.PeekChar() != -1)
        {
            var currentKey = reader.ReadString();
            var value = reader.ReadString();

            if (currentKey == key)
                return value;
        }

        return null;
    }

    public static async Task<Segment> Save(SaveSegmentModel model, CancellationToken token)
    {
        var creationDate = DateTime.Now;
        var filename = Filename(model.SegmentName);
        var metaFilename = HeaderFilename(model.SegmentName);

        var blocks = new List<(string, long)>();
        await using var writer = new FileStream(filename, FileMode.Create);

        long offset = 0;
        var memoryStream = new MemoryStream();
        var memoryWriter = new BinaryWriter(memoryStream, Encoding.UTF8, true);

        blocks.Add((model.Items.First().Key, offset));
        foreach (var item in model.Items)
        {
            if (memoryWriter.BaseStream.Position >= model.BytesPerBlock)
            {
                await CopyAndDispose(memoryStream, writer, model.Compressor, token);
                memoryStream = new MemoryStream();
                memoryWriter = new BinaryWriter(memoryStream, Encoding.UTF8, true);
                offset = writer.Position;
                blocks.Add((item.Key, offset));
            }

            memoryWriter.Write(item.Key);
            memoryWriter.Write(item.Value);
        }

        if (memoryWriter.BaseStream.Position is not 0)
            await CopyAndDispose(memoryStream, writer, model.Compressor, token);

        var header = new SegmentHeader(model.Items.Count, creationDate, model.BytesPerBlock, model.CompressorName);
        header.WriteWithBlockMap(metaFilename, blocks);

        return new Segment(filename, metaFilename, blocks, header, model.Compressor);
    }

    private static async Task CopyAndDispose(Stream source, Stream destination, ICompressor compressor,
        CancellationToken token)
    {
        source.Position = 0;
        await compressor.Compress(source, destination, token);
        await source.DisposeAsync();
    }

    private List<(string, long)> ReadBlocks()
    {
        using var reader =
            new BinaryReader(new FileStream(_headerFilename, FileMode.Open, FileAccess.Read, FileShare.Read),
                Encoding.UTF8, false);
        SegmentHeader.SkipHeaders(reader);

        var blocks = new List<(string, long)>();
        while (reader.PeekChar() != -1) blocks.Add((reader.ReadString(), reader.ReadInt64()));

        return blocks;
    }

    private static int LowerBound(IReadOnlyList<(string, long)> list, string target)
    {
        int low = 0, high = list.Count - 1;
        while (low <= high)
        {
            var mid = low + (high - low) / 2;

            if (list[mid].Item1 == target)
                return mid;
            if (string.Compare(list[mid].Item1, target, StringComparison.Ordinal) > 0)
                high = mid - 1;
            else
                low = mid + 1;
        }

        return high >= 0 && high < list.Count ? high : -1;
    }

    private static string Filename(string name)
    {
        return $"{name}.segment";
    }

    private static string HeaderFilename(string name)
    {
        return $"{name}.map";
    }

    public class SegmentWriter : IDisposable
    {
        private readonly int _bytesPerBlock;
        private readonly ICompressor _compressor;
        private readonly DateTime _creationDate;
        private readonly string _name;
        private readonly Stream _stream;

        private readonly List<(string, long)> _writerBlocks;
        private long _count;
        private MemoryStream _currentStream;

        private BinaryWriter _currentWriter;


        public SegmentWriter(string name, ICompressor compressor, int bytesPerBlock, DateTime creationDate)
        {
            _name = name;
            _compressor = compressor;
            _bytesPerBlock = bytesPerBlock;
            _creationDate = creationDate;

            var filename = Filename(_name);
            _stream = new FileStream(filename, FileMode.Create);
            _currentStream = new MemoryStream();
            _currentWriter = new BinaryWriter(_currentStream, Encoding.UTF8, false);

            _writerBlocks = new List<(string, long)>();
            _count = 0;
        }

        public void Dispose()
        {
            _stream.Dispose();
        }

        public async Task AddItem(StorageItem item, CancellationToken token)
        {
            if (_currentStream.Position is 0) _writerBlocks.Add((item.Key, _stream.Position));

            if (_currentStream.Position >= _bytesPerBlock) await Save(token);

            _currentWriter.Write(item.Key);
            _currentWriter.Write(item.Value);
            _count++;
        }

        private async Task Save(CancellationToken token)
        {
            if (_currentStream.Position is 0) return;

            _currentStream.Position = 0;
            await _compressor.Compress(_currentStream, _stream, token);
            await _currentWriter.DisposeAsync();

            _currentStream = new MemoryStream();
            _currentWriter = new BinaryWriter(_currentStream, Encoding.UTF8, false);
        }

        public async Task<Segment> Finish(ICompressorFactory factory, CancellationToken token)
        {
            await Save(token);
            var filename = Filename(_name);
            var headerFilename = HeaderFilename(_name);
            var header = new SegmentHeader(_count, _creationDate, _bytesPerBlock,
                factory.NameOfCompressor(_compressor));
            header.WriteWithBlockMap(headerFilename, _writerBlocks);

            return new Segment(filename, headerFilename, _writerBlocks, header, _compressor);
        }
    }
}