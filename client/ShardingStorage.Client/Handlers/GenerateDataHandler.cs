using Bogus;
using ShardingStorage.Client.Exceptions;
using ShardingStorage.Client.Grpc.ClientServices;

namespace ShardingStorage.Client.Handlers;

public class GenerateDataHandler : IHandler
{
    private readonly IClientService _clientService;

    public GenerateDataHandler(IClientService clientService)
    {
        _clientService = clientService;
    }
    
    public void Handle(string[] args)
    {
        if (args.Length is not 1)
            throw HandlerException.IncorrectArgumentsCount(1, args.Length);

        int count = int.Parse(args[0]);

        var faker = new Faker();
        for (int i = 0; i < count; ++i)
        {
            var key = faker.Random.Word();
            var value = faker.Random.Word();
            _clientService.Set(key, value);
            
            Console.WriteLine($"Set: Key {key}, Value: {value}");
        }
    }
}