using ShardingStorage.Core.Application.Extensions;
using ShardingStorage.Core.DataAccess.Lsm.Extensions;
using ShardingStorage.Core.DataAccess.Redis.Extensions;
using ShardingStorage.Core.Grpc.Extensions;
using ShardingStorage.Core.Web.Configurations;

namespace ShardingStorage.Core.Web.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection Configure(
        this IServiceCollection services,
        DataAccessConfiguration dataAccess, RateLimiterConfiguration rateLimiter)
    {
        switch (dataAccess.StorageType)
        {
            case DataAccessConfiguration.RedisStorageTypeName:
                services.AddShardingRedis(dataAccess.ConfigurationPath);
                break;
            case DataAccessConfiguration.LsmTreeStorageTypeName:
                services.AddShardingLsm(dataAccess.ConfigurationPath);
                break;
        }

        services.AddApplicationServices(rateLimiter.ConfigurationPath);

        services.AddControllers();

        services.AddGrpcServices();

        // services.AddEndpointsApiExplorer();
        // services.AddSwaggerGen();

        return services;
    }

    private static IServiceCollection AddStorage(this IServiceCollection services)
    {
        return services;
    }
}