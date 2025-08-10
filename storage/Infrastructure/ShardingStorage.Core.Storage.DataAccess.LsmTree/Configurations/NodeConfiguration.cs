namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Configurations;

public class NodeConfiguration
{
    public const string MasterConfigurationType = "master";
    public const string ReplicaConfigurationType = "replica";

    public string Type { get; init; } = null!;
    
    public Replica? ReplicaType { get; init; }
    public string? MasterAddress { get; init; }
    public string? Ip { get; init; }

    public enum Replica
    {
        Sync,
        Async
    }
}