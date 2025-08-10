using Microsoft.Extensions.Logging;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Configurations;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Models;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Services.Implementations.Entities;
using ShardingStorage.Core.Storage.Domain.Entities;
using ShardingStorage.Core.Storage.Domain.Models;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Services.Implementations;

public class FileService : IFileStorage, IFileMerger
{
    private readonly string _baseDirectory;
    private readonly ICompressor _compressor;
    private readonly ICompressorFactory _factory;
    private readonly ILogger<FileService> _logger;
    private readonly int _maxBytesPerBlock;

    private readonly LinkedList<Segment> _segments;
    private readonly int _segmentsPerMerge;

    public FileService(FileServiceConfiguration configuration, ICompressorFactory factory, ILogger<FileService> logger)
    {
        _factory = factory;
        _logger = logger;

        _baseDirectory = configuration.BaseDirectory;
        _maxBytesPerBlock = configuration.MaxBytesPerBlock;
        _segmentsPerMerge = configuration.SegmentsPerMerge;
        _compressor = factory.GetCompressor(configuration.Compressor);

        var files = Directory.EnumerateFiles(_baseDirectory, "*.segment");
        _segments = new LinkedList<Segment>(files.Select(f => new Segment(f[..^8], factory))
            .OrderByDescending(x => x.Header.CreationDate));

        _logger.LogInformation($"Opened Segments: {_segments.Count}");
    }

    public async Task MergeAsync(CancellationToken cancellationToken = default)
    {
        if (_segments.Count <= 1)
            return;

        _logger.LogInformation("Merging Segments");

        var mergingSegmentsCount = Math.Max(0, _segments.Count - _segmentsPerMerge);
        var mergingSegments = _segments.Skip(mergingSegmentsCount).ToList();
        var segment = await Merge(mergingSegments, default);

        for (var i = 0; i < mergingSegmentsCount; ++i) _segments.RemoveLast();

        _segments.AddLast(segment);

        foreach (var s in mergingSegments) s.Delete();

        _logger.LogInformation("Merging finished");
    }

    public async Task<StorageItem?> LoadByKeyAsync(Key key, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Looking for key {key}");

        var duplicate = _segments;

        foreach (var segment in duplicate)
        {
            var result = await segment.Get(key, cancellationToken);
            if (result is not null)
                return new StorageItem(key, result);
        }

        return null;
    }

    public async Task SaveAsync(IReadOnlyCollection<StorageItem> items, CancellationToken cancellationToken = default)
    {
        var name = Guid.NewGuid().ToString();

        _logger.LogInformation($"Saving Segment {name}");
        _segments.AddFirst(await Segment.Save(
            new SaveSegmentModel
            {
                SegmentName = GetNameWithDir(name),
                Items = items,
                Compressor = _compressor,
                CompressorName = _factory.NameOfCompressor(_compressor),
                BytesPerBlock = _maxBytesPerBlock
            }
            , cancellationToken));
    }

    private async Task<Segment> Merge(IReadOnlyCollection<Segment> segments, CancellationToken token)
    {
        var queue = new PriorityQueue<SegmentStorageItem, SegmentStorageItem>(
            Comparer<SegmentStorageItem>.Create((i1, i2) => i1.CompareTo(i2)));
        var name = Guid.NewGuid().ToString();

        using var writer = new Segment.SegmentWriter(
            GetNameWithDir(name),
            _compressor,
            _maxBytesPerBlock,
            segments.First().Header.CreationDate);

        List<SegmentReader> readers = new();
        foreach (var segment in segments)
        {
            var reader = segment.SegmentReader();
            readers.Add(reader);
            var item = new SegmentStorageItem(reader, segment.Header.CreationDate);

            queue.Enqueue(item, item);
        }

        Key? prevItem = null;
        while (queue.Count is not 0)
        {
            var item = queue.Dequeue();
            if (prevItem is not null && item.CurrentItem.Key == prevItem)
            {
                if (item.Reader.HasNext())
                {
                    await item.MoveNext(token);
                    queue.Enqueue(item, item);
                }

                continue;
            }

            prevItem = item.CurrentItem.Key;
            await writer.AddItem(item.CurrentItem, token);

            if (!item.Reader.HasNext()) continue;
            await item.MoveNext(token);
            queue.Enqueue(item, item);
        }

        foreach (var reader in readers) reader.Dispose();

        return await writer.Finish(_factory, token);
    }

    private string GetNameWithDir(string name)
    {
        return $"{_baseDirectory}/{name}";
    }

    private class SegmentStorageItem : IComparable<SegmentStorageItem>
    {
        public SegmentStorageItem(SegmentReader reader, DateTime creationDate)
        {
            Reader = reader;
            CreationDate = creationDate;
            CurrentItem = reader.ReadItem(default).Result;
        }

        public SegmentReader Reader { get; }
        private DateTime CreationDate { get; }
        public StorageItem CurrentItem { get; private set; }

        public int CompareTo(SegmentStorageItem? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;

            var result = string.Compare(CurrentItem.Key, other.CurrentItem.Key, StringComparison.Ordinal);
            if (result is not 0)
                return result;

            return -CreationDate.CompareTo(other.CreationDate);
        }

        public async Task MoveNext(CancellationToken token)
        {
            CurrentItem = await Reader.ReadItem(token);
        }
    }
}