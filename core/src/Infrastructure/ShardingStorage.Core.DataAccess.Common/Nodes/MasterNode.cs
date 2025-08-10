namespace ShardingStorage.Core.DataAccess.Common.Nodes;

public class MasterNode : INode
{
    public MasterNode(string ip, IList<ReplicaNode> replicas)
    {
        Ip = ip;
        Replicas = replicas;
    }

    public string Ip { get; }
    public IList<ReplicaNode> Replicas { get;  }
}