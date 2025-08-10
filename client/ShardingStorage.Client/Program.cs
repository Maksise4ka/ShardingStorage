using Grpc.Core;
using Grpc.Net.Client;
using ShardingStorage.Client.Exceptions;
using ShardingStorage.Client.Grpc.ClientServices;
using ShardingStorage.Client.Handlers;
using ShardingStorage.Client.Mediators;
using ShardingStorage.Client.Mediators.Implementations;
using ShardingStorage.Client.Tools;

var configuration = ConfigurationReader.ReadFromEnvironment();

using var grpcChannel = GrpcChannel.ForAddress(configuration.CoreBaseAddress);

IClientService clientService = new GrpcClientService(grpcChannel);

IMediator mediator = new Mediator()
    .AddHandler("set", new SetHandler(clientService))
    .AddHandler("get", new GetHandler(clientService))
    .AddHandler("generate", new GenerateDataHandler(clientService));

if (args.Length < 1)
{
    Console.WriteLine("args length must be greater than 1");
    return;
}

try
{
    mediator.Handle(args[0], args[1..]);
}
catch (EntityNotFoundException)
{
    Console.WriteLine("Response is empty");
}
catch (Exception ex) when (ex is MediatorException or HandlerException)
{
    Console.WriteLine(ex.Message);
}
catch (RpcException ex)
{
    Console.WriteLine("Problem with server: " + ex.Message);
}