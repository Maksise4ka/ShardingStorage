using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Extensions;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Configurations;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Extensions;
using ShardingStorage.Core.Storage.Grpc.Extensions;
using ShardingStorage.Core.Storage.Web.Configurations;

namespace ShardingStorage.Core.Storage.Web.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication Configure(this WebApplication application)
    {
        application.MapGrpcServices();

        return application;
    }

    public static async Task InitializeAsync(this WebApplication application, DataAccessConfiguration configuration)
    {
        using var scope = application.Services.CreateScope();

        // var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        // logger.LogInformation("Storage type: {Storagetype}", configuration.StorageType);
        
        await scope.StartMergerBackgroundServiceAsync(default);
    }
    
    public static WebApplication InitializeNode(this WebApplication application, NodeConfiguration configuration)
    {
        using var scope = application.Services.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Node type: {NodeType}", configuration.Type);
        
        if (configuration.Type is NodeConfiguration.ReplicaConfigurationType)
        {
            application.ConfigureReplica();
        }
        else
        {
            application.ConfigureMaster();
        }

        return application;
    }
}