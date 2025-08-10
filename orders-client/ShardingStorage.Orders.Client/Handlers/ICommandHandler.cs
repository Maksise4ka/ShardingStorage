namespace ShardingStorage.Orders.Client.Handlers;

public interface ICommandHandler
{
    void Handle(string command, string[] args);
}