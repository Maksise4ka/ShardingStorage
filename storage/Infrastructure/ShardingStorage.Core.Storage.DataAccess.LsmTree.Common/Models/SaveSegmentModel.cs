using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Services;
using ShardingStorage.Core.Storage.Domain.Entities;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Models;

public record SaveSegmentModel
{
    public required string SegmentName { get; init; }

    public required IReadOnlyCollection<StorageItem> Items { get; init; }

    public required ICompressor Compressor { get; init; }

    public required string CompressorName { get; init; }

    public required int BytesPerBlock { get; init; }
}