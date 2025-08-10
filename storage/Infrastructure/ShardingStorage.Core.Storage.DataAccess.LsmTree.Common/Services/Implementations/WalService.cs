using System.Text;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Configurations;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Wals;
using ShardingStorage.Core.Storage.Domain.Entities;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Services.Implementations;

public class WalService : IWalService
{
    private readonly string _walPath;

    public WalService(LsmTreeStorageConfiguration configuration)
    {
        _walPath = configuration.WalPath;
    }

    public void Append(StorageItem item)
    {
        var stream = new BinaryWriter(new FileStream(_walPath, FileMode.Append, FileAccess.Write), Encoding.UTF8, false);
        stream.Write(item.Key);
        stream.Write(item.Value);
        stream.Dispose();
    }

    public async Task Clear(CancellationToken cancellationToken = default)
    {
        await File.WriteAllTextAsync(_walPath, string.Empty, cancellationToken);

    }

    public async Task SetWal(Wal wal, CancellationToken cancellationToken = default)
    {
        await ClearFile(cancellationToken);
        var stream = new BinaryWriter(new FileStream(_walPath, FileMode.Append, FileAccess.Write), Encoding.UTF8, false);
        foreach (var item in wal.History)
        {
            stream.Write(item.Key);
            stream.Write(item.Value);
        }

        await stream.DisposeAsync();
    }

    public Wal RestoreWal()
    {
        var items = new List<StorageItem>();

        using var stream = new BinaryReader(new FileStream(_walPath, FileMode.Open, FileAccess.Read, FileShare.Read)
            , Encoding.UTF8, false);

        while (stream.PeekChar() != -1)
        {
            var key = stream.ReadString();
            var value = stream.ReadString();
            
            items.Add(new StorageItem(key, value));
        }

        return new Wal(items);
    }


    private async Task ClearFile(CancellationToken token)
    {
        await File.WriteAllTextAsync(_walPath, string.Empty, token);
    }
}