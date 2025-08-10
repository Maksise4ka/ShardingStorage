using ShardingStorage.Core.DataAccess.Common.Nodes;

namespace ShardingStorage.Core.DataAccess.Lsm.Nodes;

public class LsmNode : INode
{
    public LsmNode(string ip)
    {
        Ip = ip;
    }

    public string Ip { get; }
}