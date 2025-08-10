using ShardingStorage.Orders.Client.Headers;

namespace ShardingStorage.Orders.Client.Commands;

public class AddHeaderCommand : ICommand
{
    private readonly IHeadersProvider _headersProvider;

    public AddHeaderCommand(IHeadersProvider headersProvider)
    {
        _headersProvider = headersProvider;
    }

    public string Name => "add-header";
    
    public void Execute(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Invalid arguments");
            return;
        }

        var key = args[0];
        var value = args[1];
        
        _headersProvider.Add(key, value);
        
        Console.WriteLine("Added");
    }
}