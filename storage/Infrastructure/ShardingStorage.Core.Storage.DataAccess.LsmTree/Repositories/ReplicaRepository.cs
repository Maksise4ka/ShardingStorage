using ShardingStorage.Core.Storage.DataAccess.LsmTree.Abstractions.Repositories;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Abstractions.Services;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Repositories;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Wals;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Configurations;
using ShardingStorage.Core.Storage.Domain.Entities;
using ShardingStorage.Core.Storage.Domain.Models;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Repositories;

public class ReplicaRepository : IReplicaRepository
{
    private readonly StorageItemRepositoryBase _repository;
    
    public ReplicaRepository(IReplicaCommunicationService replicaService, StorageItemRepositoryBase repository,
        NodeConfiguration configuration)
    {
        _repository = repository;

        Wal wal = replicaService.Subscribe(configuration.MasterAddress!, configuration.Ip!,
            configuration.ReplicaType!.Value.ToString()).Result;
        _repository.SetWal(wal, default).Wait();
    }

    public void Append(StorageItem storageItem)
    {
        _repository.Add(storageItem);
    }

    public void Add(StorageItem item)
    {
        throw new InvalidOperationException("Can't add item to replica");
    }

    public async Task<StorageItem?> FindByKeyAsync(Key key, CancellationToken cancellationToken = default)
    {
        return await _repository.FindByKeyAsync(key, cancellationToken);
    }
}