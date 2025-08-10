using ShardingStorage.Core.DataAccess.Common.Nodes;

namespace ShardingStorage.Core.DataAccess.Common.Configurations;

public class NodeConfiguration
{
    public NodeConfiguration(IList<MasterNode> masterNodes)
    {
        MasterNodes = masterNodes;
    }

    public IList<MasterNode> MasterNodes { get; init; }
}