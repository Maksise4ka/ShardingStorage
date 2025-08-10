using ShardingStorage.Core.Domain.Entities;
using ShardingStorage.Core.Domain.Models;

namespace ShardingStorage.Core.DataAccess.Abstractions.Repositories;

public interface IStorageItemRepository
{
    Task Add(StorageItem item, CancellationToken cancellationToken = default);

    Task<StorageItem?> FindByKeyAsync(Key key, CancellationToken cancellationToken = default);
}