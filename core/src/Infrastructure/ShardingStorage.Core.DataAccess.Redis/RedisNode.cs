using ShardingStorage.Core.DataAccess.Common.Nodes;

namespace ShardingStorage.Core.DataAccess.Redis;

public class RedisNode : INode
{
    public RedisNode(string ip, string password)
    {
        Password = password;
        Ip = ip;
    }

    public string Ip { get; }
    public string Password { get; }
}