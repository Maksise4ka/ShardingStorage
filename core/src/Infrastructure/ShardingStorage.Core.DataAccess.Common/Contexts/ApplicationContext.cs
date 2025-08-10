using ShardingStorage.Core.DataAccess.Abstractions.Contexts;
using ShardingStorage.Core.DataAccess.Abstractions.Repositories;

namespace ShardingStorage.Core.DataAccess.Common.Contexts;

public class ApplicationContext : IApplicationContext
{
    public ApplicationContext(IStorageItemRepository repository)
    {
        Items = repository;
    }

    public IStorageItemRepository Items { get; }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}