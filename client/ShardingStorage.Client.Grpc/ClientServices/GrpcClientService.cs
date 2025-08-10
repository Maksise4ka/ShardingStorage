using Grpc.Core;
using Grpc.Net.Client;
using ShardingStorage.Client.Grpc.Protos;

namespace ShardingStorage.Client.Grpc.ClientServices;

public class GrpcClientService : IClientService
{
    private readonly GrpcChannel _grpcChannel;

    public GrpcClientService(GrpcChannel grpcChannel)
    {
        _grpcChannel = grpcChannel;
    }

    public GetResponse Get(string key)
    {
        var request = new GetRequest
        {
            Key = key
        };
        var client = InitializeClient();

        var headers = new Metadata
        {
            { "userIP", "asd" }
        };
        
        return client.Get(request, headers);
    }

    public void Set(string key, string value)
    {
        var request = new SetRequest
        {
            Key = key,
            Value = value
        };
        var client = InitializeClient();
        
        var headers = new Metadata
        {
            { "userIP", "asd" }
        };

        client.Set(request, headers);
    }

    private StorageService.StorageServiceClient InitializeClient()
    {
        return new StorageService.StorageServiceClient(_grpcChannel);
    }
}