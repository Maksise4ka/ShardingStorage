using ShardingStorage.Core.Storage.Domain.Models;

namespace ShardingStorage.Core.Storage.Domain.Entities;

public class StorageItem
{
    public StorageItem(Key key, string value)
    {
        Key = key;
        Value = value;
    }

    public Key Key { get; }

    public string Value { get; set; }
}