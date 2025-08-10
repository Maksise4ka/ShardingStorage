namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Configurations;

public class LsmTreeStorageConfiguration
{
    public int MaxItemsCapacityInMemory { get; init; }

    public int MergingMillisecondsDelay { get; init; }
    
    public int FirstMergingMillisecondsDelay { get; init; }

    public string WalPath { get; init; } = null!;
}