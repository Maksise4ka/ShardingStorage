using ShardingStorage.Core.Storage.Domain.Models;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Services.Implementations.Entities;

public interface ISegment
{
    ICollection<Key> Keys { get; }

    DateTime CreationDate { get; }
    bool KeyExists(Key key);

    string? GetValue(Key key);
}