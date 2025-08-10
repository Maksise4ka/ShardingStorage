using ShardingStorage.Core.Storage.Application.Extensions;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Extensions;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Configurations;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Extensions;
using ShardingStorage.Core.Storage.Web.Configurations;

namespace ShardingStorage.Core.Storage.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection Configure(this IServiceCollection services, DataAccessConfiguration configuration)
    {
        services.AddApplicationServices();
        // services.AddGrpcServices();
        services.AddLsmTreeStorage(configuration.LsmTreeStorageConfiguration, configuration.FileServiceConfiguration);
        services.ConfigureNode(configuration.NodeConfiguration);

        return services;
    }
}