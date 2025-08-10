using Grpc.Core;
using ShardingStorage.Core.Application.Contracts.Services;
using ShardingStorage.Grpc.Contracts;

namespace ShardingStorage.Core.Grpc.Services;

public class RateLimiterGrpcService : RateLimiterService.RateLimiterServiceBase
{
    private readonly IRateLimitService _service;

    public RateLimiterGrpcService(IRateLimitService service)
    {
        _service = service;
    }

    public override async Task<UpdateResponse> Update(UpdateRequest request, ServerCallContext context)
    {
        var descriptors = request.Descriptors.ToDictionary(d => d.Key, d => d.Value);

        var result = await _service.Update(descriptors, default);

        return new UpdateResponse { Allowed = result };
    }
}