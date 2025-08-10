using ShardingStorage.Core.Storage.Application.Contracts.Services;
using ShardingStorage.Core.Storage.DataAccess.Abstractions.Contexts;
using ShardingStorage.Core.Storage.Domain.Entities;

namespace ShardingStorage.Core.Storage.Application.Services;

internal class StorageService : IStorageService
{
    private readonly IApplicationContext _context;
    
    public StorageService(IApplicationContext context)
    {
        _context = context;
    }

    public async Task SetAsync(string key, string value, CancellationToken cancellationToken = default)
    {
        // var item = await _context.Items.FindByKeyAsync(key, cancellationToken);

        // if (item is null)
        _context.Items.Add(new StorageItem(key, value));
        // else
        // item.Value = value;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<string?> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        var item = await _context.Items.FindByKeyAsync(key, cancellationToken);

        return item?.Value;
    }
}