using ShardingStorage.Client.Exceptions;
using ShardingStorage.Client.Grpc.ClientServices;

namespace ShardingStorage.Client.Handlers;

public class SetHandler : IHandler
{
    private readonly IClientService _clientService;

    public SetHandler(IClientService clientService)
    {
        _clientService = clientService;
    }

    public void Handle(string[] args)
    {
        if (args.Length is not 2)
            throw HandlerException.IncorrectArgumentsCount(2, args.Length);

        _clientService.Set(args[0], args[1]);
        Console.WriteLine("handled");
    }
}