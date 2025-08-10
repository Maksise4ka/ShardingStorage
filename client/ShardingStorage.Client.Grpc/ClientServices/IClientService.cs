using ShardingStorage.Client.Grpc.Protos;

namespace ShardingStorage.Client.Grpc.ClientServices;

public interface IClientService
{
    GetResponse Get(string key);
    void Set(string key, string value);
}