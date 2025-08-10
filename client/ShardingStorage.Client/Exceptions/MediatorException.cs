namespace ShardingStorage.Client.Exceptions;

public class MediatorException : Exception
{
    private MediatorException(string? message)
        : base(message)
    {
    }

    public static MediatorException CommandAlreadyExists(string command)
    {
        return new MediatorException($"Command with name {command} already exists");
    }

    public static MediatorException CommandNotExists(string command)
    {
        return new MediatorException($"Command with name {command} doesn't exist");
    }
}