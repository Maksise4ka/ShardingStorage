using ShardingStorage.Client.Exceptions;
using ShardingStorage.Client.Grpc.ClientServices;

namespace ShardingStorage.Client.Handlers;

public class GetHandler : IHandler
{
    private readonly IClientService _clientService;

    public GetHandler(IClientService clientService)
    {
        _clientService = clientService;
    }

    public void Handle(string[] args)
    {
        if (args.Length is not 1)
            throw HandlerException.IncorrectArgumentsCount(1, args.Length);

        var result = _clientService.Get(args[0]);
        if (result.Value is null)
            throw new EntityNotFoundException();

        Console.WriteLine(result.Value);
    }
}