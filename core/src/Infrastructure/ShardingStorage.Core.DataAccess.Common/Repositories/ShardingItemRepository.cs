using Microsoft.Extensions.Logging;
using ShardingStorage.Core.DataAccess.Abstractions.Repositories;
using ShardingStorage.Core.DataAccess.Common.Configurations;
using ShardingStorage.Core.DataAccess.Common.Hash;
using ShardingStorage.Core.DataAccess.Common.Nodes;
using ShardingStorage.Core.Domain.Entities;
using ShardingStorage.Core.Domain.Models;

namespace ShardingStorage.Core.DataAccess.Common.Repositories;

public abstract class ShardingItemRepository : IStorageItemRepository
{
    private readonly IList<MasterNode> _masters;
    private readonly IHashStrategy _hashStrategy;
    private readonly ILogger<ShardingItemRepository> _logger;

    protected ShardingItemRepository(IHashStrategy hashStrategy, NodeConfiguration configuration,
        ILogger<ShardingItemRepository> logger)
    {
        _hashStrategy = hashStrategy;
        _logger = logger;
        _masters = configuration.MasterNodes;
    }

    public async Task Add(StorageItem item, CancellationToken cancellationToken = default)
    {
        int masterNumber = _hashStrategy.ComputeHash(item.Key, _masters.Count);
        await Add(item, _masters[masterNumber], cancellationToken);
    }

    public Task<StorageItem?> FindByKeyAsync(Key key, CancellationToken cancellationToken = default)
    {
        int masterNumber = _hashStrategy.ComputeHash(key, _masters.Count);
        var master = _masters[masterNumber];

        var list = master.Replicas.ToList<INode>();
        list.Add(master);
        INode node = PickNode(list);

        _logger.LogInformation(
            $"master: {masterNumber} with {_masters[masterNumber].Replicas.Count} replicas, node: {node.Ip}");
        return FindByKeyAsync(key, node, cancellationToken);
    }

    protected abstract Task Add(StorageItem item, MasterNode node, CancellationToken cancellationToken = default);
    protected abstract Task<StorageItem?> FindByKeyAsync(Key key, INode node, CancellationToken cancellationToken = default);

    private INode PickNode(IList<INode> nodes)
        => nodes[Math.Abs(Guid.NewGuid().GetHashCode()) % nodes.Count];
}