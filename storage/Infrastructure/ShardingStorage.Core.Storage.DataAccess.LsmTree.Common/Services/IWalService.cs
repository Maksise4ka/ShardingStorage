using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Wals;
using ShardingStorage.Core.Storage.Domain.Entities;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Services;

public interface IWalService
{
    void Append(StorageItem item);
    Task Clear(CancellationToken cancellationToken = default);

    Wal RestoreWal();
    Task SetWal(Wal wal, CancellationToken cancellationToken = default);
}