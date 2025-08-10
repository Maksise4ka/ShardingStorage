namespace ShardingStorage.Core.Storage.Domain.Exceptions.Models;

public class InvalidKeyException : DomainException
{
    public InvalidKeyException()
        : base("Key cannot be empty or whitespace")
    {
    }
}