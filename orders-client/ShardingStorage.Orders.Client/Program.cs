using ShardingStorage.Orders.Client.Clients;
using ShardingStorage.Orders.Client.Commands;
using ShardingStorage.Orders.Client.Handlers;
using ShardingStorage.Orders.Client.Headers;

var baseAddress = Environment.GetEnvironmentVariable("BASE_ADDRESS")
                  ?? "https://localhost:7089";

var httpClient = new HttpClient
{
    BaseAddress = new Uri(baseAddress, UriKind.Absolute)
};

IHeadersProvider headersProvider = new HeadersProvider();

IOrderClient orderClient = new OrderClient(httpClient, headersProvider);

ICommandHandler commandHandler = new CommandHandler()
    .AddCommand(new AddHeaderCommand(headersProvider))
    .AddCommand(new GenerateCommand(orderClient))
    .AddCommand(new GetCommand(orderClient));

while (true)
{
    var input = Console.ReadLine();
    
    if (input is null or "exit")
        break;

    var data = input.Split(' ');
    
    commandHandler.Handle(data[0], data[1..]);
}