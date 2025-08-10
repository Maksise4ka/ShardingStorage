using Microsoft.Extensions.DependencyInjection;
using ShardingStorage.Core.Storage.Grpc.Interceptors;

namespace ShardingStorage.Core.Storage.Grpc.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGrpcServices(this IServiceCollection services)
    {
        services.AddGrpc(x => x.Interceptors.Add<ExceptionHandlingInterceptor>());

        return services;
    }
}