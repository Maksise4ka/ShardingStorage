using Microsoft.Extensions.DependencyInjection;
using ShardingStorage.Core.Grpc.Interceptors;

namespace ShardingStorage.Core.Grpc.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGrpcServices(
        this IServiceCollection services)
    {
        services.AddGrpc(x =>
        {
            x.Interceptors.Add<ExceptionHandlingInterceptor>();
        });
        
        // services.AddSingleton(rateLimitingConfiguration);

        return services;
    }
}