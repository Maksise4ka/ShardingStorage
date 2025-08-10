namespace ShardingStorage.Core.DataAccess.Common.Nodes;

public class ReplicaNode : INode
{
    public ReplicaNode(string ip)
    {
        Ip = ip;
    }

    public string Ip { get; }
}