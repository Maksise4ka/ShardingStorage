using ShardingStorage.Core.Storage.Domain.Entities;
using ShardingStorage.Core.Storage.Domain.Models;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Services;

public interface IFileStorage
{
    Task<StorageItem?> LoadByKeyAsync(Key key, CancellationToken cancellationToken = default);

    Task SaveAsync(IReadOnlyCollection<StorageItem> items, CancellationToken cancellationToken = default);
}