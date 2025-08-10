namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Configurations;

public class FileServiceConfiguration
{
    public string BaseDirectory { get; init; } = null!;

    public int MaxBytesPerBlock { get; init; }

    public int SegmentsPerMerge { get; init; }

    public string Compressor { get; init; } = null!;
}