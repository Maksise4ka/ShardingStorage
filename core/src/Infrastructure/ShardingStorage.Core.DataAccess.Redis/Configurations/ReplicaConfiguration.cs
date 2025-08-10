using ShardingStorage.Core.DataAccess.Common.Nodes;

namespace ShardingStorage.Core.DataAccess.Redis.Configurations;

public class ReplicaConfiguration
{
    public string Name { get; init; } = null!;
    public string Replica { get; init; } = null!;
    public string Password { get; init; } = null!;
    
    public ReplicaNode AsNode() => new(Replica);
}