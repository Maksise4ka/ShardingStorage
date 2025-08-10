using ShardingStorage.Core.Storage.DataAccess.Abstractions.Repositories;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Configurations;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Services;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Wals;
using ShardingStorage.Core.Storage.Domain.Entities;
using ShardingStorage.Core.Storage.Domain.Models;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Repositories;

public class StorageItemRepositoryBase : IStorageItemRepository
{
    private readonly LsmTreeStorageConfiguration _configuration;
    private readonly IFileStorage _fileStorage;
    private SortedDictionary<Key, StorageItem> _items;
    private int _totalMemory;
    private readonly IWalService _walService;

    public StorageItemRepositoryBase(IFileStorage fileStorage, IWalService walService,
        LsmTreeStorageConfiguration configuration)
    {
        _fileStorage = fileStorage;
        _configuration = configuration;
        _walService = walService;

        (_items, _totalMemory) = Restore(_walService.RestoreWal());
    }

    public void Add(StorageItem item)
    {
        if (_totalMemory >= _configuration.MaxItemsCapacityInMemory)
        {
            DumpAsync().Wait();
        }
        _walService.Append(item);

        if (_items.TryGetValue(item.Key, out var existingItem))
        {
            existingItem.Value = item.Value;
            _totalMemory += item.Value.Length - existingItem.Value.Length;
            return;
        }

        _totalMemory += item.Key.Value.Length + item.Value.Length;
        _items.Add(item.Key, item);
    }

    public async Task<StorageItem?> FindByKeyAsync(Key key, CancellationToken cancellationToken = default)
    {
        _items.TryGetValue(key, out var item);

        item ??= await _fileStorage.LoadByKeyAsync(key, cancellationToken);

        return item;
    }

    public async Task SetWal(Wal wal, CancellationToken token)
    {
        await _walService.SetWal(wal, token);
        (_items, _totalMemory) = Restore(wal);
    }

    public Wal GetWal()
        => _walService.RestoreWal();

    private async Task ClearMemTable(CancellationToken cancellationToken = default)
    {
        await _walService.Clear(cancellationToken);

        _items.Clear();
        _totalMemory = 0;
    }
    
    private async Task DumpAsync(CancellationToken cancellationToken = default)
    {
        await _fileStorage.SaveAsync(_items.Values, cancellationToken);
        await ClearMemTable(cancellationToken);
    }

    private static (SortedDictionary<Key, StorageItem>, int) Restore(Wal wal)
    {
        var items = new SortedDictionary<Key, StorageItem>();
        var totalMemory = 0;

        foreach (var item in wal.History)
        {
            string key = item.Key;
            var value = item.Value;

            if (items.TryGetValue(key, out var existingItem))
            {
                existingItem.Value = value;
                totalMemory += value.Length - existingItem.Value.Length;
            }
            else
            {
                totalMemory += key.Length + value.Length;
                items.Add(key, new StorageItem(key, value));
            }
        }

        return (items, totalMemory);
    }
}