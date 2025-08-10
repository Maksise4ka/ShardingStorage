using ShardingStorage.Core.Storage.Domain.Entities;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Wals;

public record Wal(IReadOnlyList<StorageItem> History);