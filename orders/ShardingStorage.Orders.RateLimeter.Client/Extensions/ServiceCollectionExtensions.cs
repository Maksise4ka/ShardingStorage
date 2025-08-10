using Microsoft.Extensions.DependencyInjection;
using ShardingStorage.Grpc.Contracts;
using ShardingStorage.Orders.RateLimeter.Client.Clients;

namespace ShardingStorage.Orders.RateLimeter.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddRateLimiterClient(this IServiceCollection services, string? address)
    {
        if (address is null)
        {
            services.AddSingleton<IRateLimeterClient, LocalRateLimiter>();
            return;
        }
        
        services.AddScoped<IRateLimeterClient, RateLimiterClient>();

        services.AddGrpcClient<RateLimiterService.RateLimiterServiceClient>(x =>
        {
            x.Address = new Uri(address);
        });
    }
}