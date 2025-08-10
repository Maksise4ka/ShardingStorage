using System.Text;
using ShardingStorage.Core.Storage.Domain.Entities;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Services.Implementations.Entities;

public class SegmentReader : IDisposable
{
    private readonly IList<(string, long)> _blocks;
    private readonly ICompressor _compressor;
    private readonly SegmentHeader _header;
    private readonly Stream _stream;

    private long _count;
    private int _currentBlock;
    private BinaryReader _currentReader;

    public SegmentReader(string filename, SegmentHeader header, ICompressor compressor, IList<(string, long)> blocks)
    {
        _header = header;
        _compressor = compressor;
        _blocks = blocks;

        _count = 0;
        _stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);

        _currentReader = new BinaryReader(new MemoryStream());
        _currentBlock = 0;
    }

    public void Dispose()
    {
        _stream.Dispose();
    }

    public async Task<StorageItem> ReadItem(CancellationToken token)
    {
        if (!HasNext())
            throw new Exception("Can't read");

        if (_currentReader.PeekChar() == -1)
        {
            _currentReader.Dispose();
            var offset = _blocks[_currentBlock].Item2;
            var nextOffset = _currentBlock + 1 == _blocks.Count ? _stream.Length : _blocks[_currentBlock + 1].Item2;
            var totalLength = (int)(nextOffset - offset);

            var buffer = new byte[totalLength];
            var _ = await _stream.ReadAsync(buffer.AsMemory(0, totalLength), token);

            var memoryStream = new MemoryStream(buffer);
            var decompressedStream = await _compressor.Decompress(memoryStream, token);
            await memoryStream.DisposeAsync();

            _currentBlock++;
            _currentReader = new BinaryReader(decompressedStream, Encoding.UTF8, false);
        }

        var key = _currentReader.ReadString();
        var value = _currentReader.ReadString();
        _count++;

        return new StorageItem(key, value);
    }

    public bool HasNext()
    {
        return _count < _header.Count;
    }
}