using ShardingStorage.Core.Storage.DataAccess.Abstractions.Repositories;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Wals;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Abstractions.Repositories;

public interface IMasterRepository : IStorageItemRepository
{
    Wal Subscribe(string replicaIp, string replicaType);
}