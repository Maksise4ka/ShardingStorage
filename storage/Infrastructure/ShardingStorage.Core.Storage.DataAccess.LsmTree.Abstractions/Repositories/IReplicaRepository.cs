using ShardingStorage.Core.Storage.DataAccess.Abstractions.Repositories;
using ShardingStorage.Core.Storage.Domain.Entities;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Abstractions.Repositories;

public interface IReplicaRepository : IStorageItemRepository
{
    void Append(StorageItem storageItem);
}