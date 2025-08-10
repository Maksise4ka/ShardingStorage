namespace ShardingStorage.Orders.Client.Commands;

public interface ICommand
{
    string Name { get; }

    void Execute(string[] args);
}