using ShardingStorage.Orders.Client.Commands;

namespace ShardingStorage.Orders.Client.Handlers;

public class CommandHandler : ICommandHandler
{
    private readonly Dictionary<string, ICommand> _commands = new();

    public CommandHandler AddCommand(ICommand command)
    {
        _commands[command.Name] = command;

        return this;
    }
    
    public void Handle(string commandName, string[] args)
    {
        _commands.TryGetValue(commandName, out var command);

        if (command is null)
        {
            Console.WriteLine("Invalid command");
            return;
        }
        
        command.Execute(args);
    }
}