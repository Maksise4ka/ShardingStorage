using Microsoft.Extensions.Logging;
using ShardingStorage.Core.DataAccess.Common.Configurations;
using ShardingStorage.Core.DataAccess.Common.Hash;
using ShardingStorage.Core.DataAccess.Common.Nodes;
using ShardingStorage.Core.DataAccess.Common.Repositories;
using ShardingStorage.Core.Domain.Entities;
using ShardingStorage.Core.Domain.Models;
using StackExchange.Redis;

namespace ShardingStorage.Core.DataAccess.Redis.Repositories;

public class StorageItemRepository : ShardingItemRepository
{
    private readonly IConnectionMultiplexer _connection;
    
    public StorageItemRepository(IHashStrategy hashStrategy, IConnectionMultiplexer connection, NodeConfiguration configuration, ILogger<ShardingItemRepository> logger)
        : base(hashStrategy, configuration, logger)
    {
        _connection = connection;
    }

    protected override async Task Add(StorageItem item, MasterNode node, CancellationToken cancellationToken = default)
    {
        var server = _connection.GetServer(node.Ip);

        await server.ExecuteAsync("set", new RedisKey(item.Key), new RedisValue(item.Value));
    }

    protected override async Task<StorageItem?> FindByKeyAsync(Key key, INode node, CancellationToken cancellationToken = default)
    {
        var server = _connection.GetServer(node.Ip);
        var result = await server.ExecuteAsync("get", new RedisKey(key));
        
        var item = result.IsNull ? null : new StorageItem(key, result.ToString());
        
        return item;
    }
}