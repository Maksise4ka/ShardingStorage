using Microsoft.AspNetCore.Builder;
using ShardingStorage.Core.Grpc.Services;

namespace ShardingStorage.Core.Grpc.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapGrpcServices(this WebApplication application)
    {
        application.MapGrpcService<RateLimiterGrpcService>();

        return application;
    }
}