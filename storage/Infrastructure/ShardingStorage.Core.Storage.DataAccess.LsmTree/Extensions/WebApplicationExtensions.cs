using Microsoft.AspNetCore.Builder;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Configurations;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Listeners;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureMaster(this WebApplication application)
    {
        application.MapGrpcService<MasterListener>();

        return application;
    }
    
    public static WebApplication ConfigureReplica(this WebApplication application)
    {
        application.MapGrpcService<ReplicaListener>();

        return application;
    }
}