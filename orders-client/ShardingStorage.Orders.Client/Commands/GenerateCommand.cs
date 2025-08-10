using ShardingStorage.Orders.Client.Clients;
using ShardingStorage.Orders.Client.Models;
using ShardingStorage.Orders.Client.Requests;

namespace ShardingStorage.Orders.Client.Commands;

public class GenerateCommand : ICommand
{
    private readonly IOrderClient _orderClient;

    public GenerateCommand(IOrderClient orderClient)
    {
        _orderClient = orderClient;
    }

    public string Name => "generate";
    
    public void Execute(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Invalid arguments");
            return;
        }

        var count = int.Parse(args[0]);
        
        for (var i = 0; i < count; ++i)
            try
            {
                _orderClient.Create(new CreateOrderRequest(new List<OrderItem>()));
                Console.WriteLine($"Created {i}");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
    }
}