using Microsoft.AspNetCore.Builder;
using ShardingStorage.Core.Storage.Grpc.Services;

namespace ShardingStorage.Core.Storage.Grpc.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapGrpcServices(this WebApplication application)
    {
        application.MapGrpcService<StorageGrpcService>();

        return application;
    }
}