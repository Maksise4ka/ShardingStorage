using ShardingStorage.Core.Domain.Exceptions.Models;

namespace ShardingStorage.Core.Domain.Models;

public readonly record struct Key : IComparable<Key>
{
    public Key(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidKeyException();

        Value = value;
    }

    public string Value { get; }

    public int CompareTo(Key other)
    {
        return string.CompareOrdinal(Value, other.Value);
    }

    public static implicit operator string(Key key)
    {
        return key.Value;
    }

    public static implicit operator Key(string value)
    {
        return new Key(value);
    }
}