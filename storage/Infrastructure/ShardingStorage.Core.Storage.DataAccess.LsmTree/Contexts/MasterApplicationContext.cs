using ShardingStorage.Core.Storage.DataAccess.Abstractions.Contexts;
using ShardingStorage.Core.Storage.DataAccess.Abstractions.Repositories;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Abstractions.Repositories;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Contexts;

public class MasterApplicationContext : IApplicationContext
{
    public MasterApplicationContext(IMasterRepository repository)
    {
        Items = repository;
    }
    
    public IStorageItemRepository Items { get; }
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}