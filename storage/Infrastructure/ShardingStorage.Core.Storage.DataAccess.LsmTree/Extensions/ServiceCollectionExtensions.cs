using Microsoft.Extensions.DependencyInjection;
using ShardingStorage.Core.Storage.DataAccess.Abstractions.Contexts;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Abstractions.Repositories;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Abstractions.Services;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Clients;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Configurations;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Contexts;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Repositories;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureNode(this IServiceCollection services, NodeConfiguration configuration)
    {
        switch (configuration.Type)
        {
            case NodeConfiguration.MasterConfigurationType:
                services.ConfigureMaster();
                break;
            case NodeConfiguration.ReplicaConfigurationType:
                services.ConfigureReplica(configuration);
                break;
        }
        return services;
    }

    private static IServiceCollection ConfigureMaster(this IServiceCollection services)
    {
        services.AddGrpc();
        
        services.AddSingleton<IMasterCommunicationService, MasterCommunicationService>();
        services.AddSingleton<IMasterRepository, MasterRepository>();
        services.AddScoped<IApplicationContext, MasterApplicationContext>();
        
        return services;
    }

    private static IServiceCollection ConfigureReplica(this IServiceCollection services, NodeConfiguration configuration)
    {
        services.AddGrpc();
        
        services.AddSingleton<IReplicaCommunicationService, ReplicaCommunicationService>();
        services.AddSingleton<NodeConfiguration>(_ => configuration);
        services.AddSingleton<IReplicaRepository, ReplicaRepository>();
        services.AddScoped<IApplicationContext, ReplicaApplicationContext>();
        
        return services;
    }
}