using ShardingStorage.Core.DataAccess.Common.Nodes;
using StackExchange.Redis;

namespace ShardingStorage.Core.DataAccess.Redis.Configurations;

public class MasterConfiguration
{
    public string Name { get; init; } = null!;
    public string Master { get; init; } = null!;
    public string Password { get; init; } = null!;
    public List<ReplicaConfiguration> Replicas { get; init; } = null!;

    public MasterNode AsNode()
        => new MasterNode(Master, Replicas.Select(r => r.AsNode()).ToList());
}