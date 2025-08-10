using ShardingStorage.Core.Storage.DataAccess.Abstractions.Repositories;

namespace ShardingStorage.Core.Storage.DataAccess.Abstractions.Contexts;

public interface IApplicationContext
{
    IStorageItemRepository Items { get; }

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}