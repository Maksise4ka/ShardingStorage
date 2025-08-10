using System.Text.Json;
using ShardingStorage.Orders.Client.Clients;

namespace ShardingStorage.Orders.Client.Commands;

public class GetCommand : ICommand
{
    private readonly IOrderClient _orderClient;

    public GetCommand(IOrderClient orderClient)
    {
        _orderClient = orderClient;
    }

    public string Name => "get";
    
    public void Execute(string[] args)
    {
        if (args.Length != 0)
        {
            Console.WriteLine("Inavalid arguments");
            return;
        }

        try
        {
            var orders = _orderClient.Get();
            Console.WriteLine(JsonSerializer.Serialize(orders));
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}