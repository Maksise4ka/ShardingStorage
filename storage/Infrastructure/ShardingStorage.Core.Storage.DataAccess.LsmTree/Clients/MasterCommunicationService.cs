using Grpc.Net.Client;
using ShardingStorage.Core.Storage.Data.Access.LsmTree.Grpc;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Abstractions.Services;
using StorageItem = ShardingStorage.Core.Storage.Domain.Entities.StorageItem;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Clients;

public class MasterCommunicationService : IMasterCommunicationService
{
    public void Append(string replicaIp, StorageItem storageItem)
    {
        var client = InitializeClient(replicaIp);
        client.Append(new AppendRequest()
        {
            Item = new Data.Access.LsmTree.Grpc.StorageItem()
            {
                Key = storageItem.Key,
                Value = storageItem.Value
            }
        });
    }

    public async Task AppendAsync(string replicaIp, StorageItem storageItem)
    {
        var client = InitializeClient(replicaIp);
        await client.AppendAsync(new AppendRequest()
        {
            Item = new Data.Access.LsmTree.Grpc.StorageItem()
            {
                Key = storageItem.Key,
                Value = storageItem.Value
            }
        });
    }

    private ReplicaService.ReplicaServiceClient InitializeClient(string ip)
    {
        var grpcChannel = GrpcChannel.ForAddress(ip);
        return new ReplicaService.ReplicaServiceClient(grpcChannel);
    }
}