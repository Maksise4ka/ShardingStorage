using ShardingStorage.Core.DataAccess.Abstractions.Repositories;

namespace ShardingStorage.Core.DataAccess.Abstractions.Contexts;

public interface IApplicationContext
{
    IStorageItemRepository Items { get; }

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}