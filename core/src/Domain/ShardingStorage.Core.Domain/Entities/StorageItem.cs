using ShardingStorage.Core.Domain.Models;

namespace ShardingStorage.Core.Domain.Entities;

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