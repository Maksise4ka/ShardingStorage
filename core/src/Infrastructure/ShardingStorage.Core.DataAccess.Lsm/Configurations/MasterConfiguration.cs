using ShardingStorage.Core.DataAccess.Common.Nodes;

namespace ShardingStorage.Core.DataAccess.Lsm.Configurations;

public class MasterConfiguration
{
    public string Name { get; init; } = null!;
    public string Master { get; init; } = null!;
    public List<String> Replicas { get; init; } = null!;

    public MasterNode AsNode()
        => new MasterNode(Master, Replicas.Select(r => new ReplicaNode(r)).ToList());
}