using ShardingStorage.Core.Storage.Domain.Entities;
using ShardingStorage.Core.Storage.Domain.Models;

namespace ShardingStorage.Core.Storage.DataAccess.Abstractions.Repositories;

public interface IStorageItemRepository
{
    void Add(StorageItem item);

    Task<StorageItem?> FindByKeyAsync(Key  key, CancellationToken cancellationToken = default);
}