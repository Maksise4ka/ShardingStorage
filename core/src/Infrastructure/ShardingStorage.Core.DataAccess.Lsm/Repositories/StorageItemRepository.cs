using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using ShardingStorage.Core.DataAccess.Common.Configurations;
using ShardingStorage.Core.DataAccess.Common.Hash;
using ShardingStorage.Core.DataAccess.Common.Nodes;
using ShardingStorage.Core.DataAccess.Common.Repositories;
using ShardingStorage.Core.DataAccess.Lsm.Services;
using ShardingStorage.Core.Domain.Entities;
using ShardingStorage.Core.Domain.Models;

namespace ShardingStorage.Core.DataAccess.Lsm.Repositories;

public class StorageItemRepository : ShardingItemRepository
{
    public StorageItemRepository(IHashStrategy hashStrategy, NodeConfiguration configuration, ILogger<ShardingItemRepository> logger) 
        : base(hashStrategy, configuration, logger)
    {
    }

    protected override async Task Add(StorageItem item, MasterNode node, CancellationToken cancellationToken = default)
    {
        var client = InitializeClient(node.Ip);
        _ = await client.SetAsync(new SetRequest { Key = item.Key, Value = item.Value }, cancellationToken: cancellationToken);
    }

    protected override async Task<StorageItem?> FindByKeyAsync(Key key, INode node, CancellationToken cancellationToken = default)
    {
        var client = InitializeClient(node.Ip);
        var response = await client.GetAsync(new GetRequest() {Key = key}, cancellationToken: cancellationToken);
        StorageItem? item = null;
        if (response.Value is not null)
        {
            item = new StorageItem(key, response.Value);
        }
        
        return item;
    }

    private static StorageService.StorageServiceClient InitializeClient(string ip)
    {
        var grpcChannel = GrpcChannel.ForAddress(ip);

        return new StorageService.StorageServiceClient(grpcChannel);
    }
}