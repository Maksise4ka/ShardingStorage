using ShardingStorage.Core.Storage.DataAccess.LsmTree.Abstractions.Repositories;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Abstractions.Services;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Repositories;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Wals;
using ShardingStorage.Core.Storage.Domain.Entities;
using ShardingStorage.Core.Storage.Domain.Models;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Repositories;

public class MasterRepository : IMasterRepository
{
    private readonly IMasterCommunicationService _masterService;
    private readonly StorageItemRepositoryBase _repository;

    private readonly IList<string> _syncReplicas;
    // private readonly IList<string> _asyncReplicas;

    public MasterRepository(IMasterCommunicationService masterService, StorageItemRepositoryBase repository)
    {
        _masterService = masterService;
        _repository = repository;

        _syncReplicas = new List<string>();
        // _asyncReplicas = new List<string>();
    }
    
    // TODO: think of how to work with async replicas
    public Wal Subscribe(string replicaIp, string replicaType)
    {
        _syncReplicas.Add(replicaIp);
        return _repository.GetWal();
    }

    public void Add(StorageItem item)
    {
        _repository.Add(item);

        foreach (var replica in _syncReplicas)
        {
            _masterService.Append(replica, item);
        }
    }

    public async Task<StorageItem?> FindByKeyAsync(Key key, CancellationToken cancellationToken = default)
    {
       return await _repository.FindByKeyAsync(key, cancellationToken);
    }
}
