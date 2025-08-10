using ShardingStorage.Orders.Client.Headers;

namespace ShardingStorage.Orders.Client.Commands;

public class RemoveHeaderCommand : ICommand
{
    private readonly IHeadersProvider _headersProvider;

    public RemoveHeaderCommand(IHeadersProvider headersProvider)
    {
        _headersProvider = headersProvider;
    }

    public string Name => "remove-header";

    public void Execute(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Invalid arguments");
            return;
        }
        
        _headersProvider.Remove(args[0]);
    }
}