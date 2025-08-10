using Grpc.Net.Client;
using ShardingStorage.Core.Storage.Data.Access.LsmTree.Grpc;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Abstractions.Services;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Wals;
using StorageItem = ShardingStorage.Core.Storage.Domain.Entities.StorageItem;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Clients;

public class ReplicaCommunicationService : IReplicaCommunicationService
{
    public async Task<Wal> Subscribe(string masterIp, string replicaIp, string replicaType)
    {
        var client = InitializeClient(masterIp);

        var wal = await client.SubscribeAsync(new SubscribeRequest()
        {
            ReplicaIp = replicaIp,
            ReplicaType = ReplicaType.Sync // TODO:
        });

        return new Wal(wal.Items.Select(x => new StorageItem(x.Key, x.Value)).ToList().AsReadOnly());
    }
    
    private MasterService.MasterServiceClient InitializeClient(string ip)
    {
        var grpcChannel = GrpcChannel.ForAddress(ip);
        return new MasterService.MasterServiceClient(grpcChannel);
    }
}