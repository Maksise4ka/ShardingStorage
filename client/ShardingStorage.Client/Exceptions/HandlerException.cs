namespace ShardingStorage.Client.Exceptions;

public class HandlerException : Exception
{
    private HandlerException(string? message)
        : base(message)
    {
    }

    public static HandlerException IncorrectArgumentsCount(int expected, int actual)
    {
        return new HandlerException($"There must be {expected} arguments, but {actual} received");
    }
}