using Grpc.Core;
using Microsoft.Extensions.Logging;
using ShardingStorage.Core.Storage.Application.Contracts.Services;
using ShardingStorage.Grpc.Contracts;

namespace ShardingStorage.Core.Storage.Grpc.Services;

internal class StorageGrpcService : StorageService.StorageServiceBase
{
    private readonly IStorageService _storageService;
    private readonly ILogger<StorageGrpcService> _logger;

    public StorageGrpcService(IStorageService storageService, ILogger<StorageGrpcService> logger)
    {
        _storageService = storageService;
        _logger = logger;
    }

    public override async Task<GetResponse> Get(GetRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"Get item {request.Key}");
        var value = await _storageService.GetAsync(request.Key, context.CancellationToken);

        return new GetResponse { Value = value };
    }

    public override async Task<SetResponse> Set(SetRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"Set item {request.Key}:{request.Value}");
        await _storageService.SetAsync(request.Key, request.Value, context.CancellationToken);

        return new SetResponse();
    }
}