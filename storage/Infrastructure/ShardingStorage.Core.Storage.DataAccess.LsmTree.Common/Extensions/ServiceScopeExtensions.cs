using Microsoft.Extensions.DependencyInjection;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.BackgroundServices;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Extensions;

public static class ServiceScopeExtensions
{
    public static Task StartMergerBackgroundServiceAsync(
        this IServiceScope scope,
        CancellationToken cancellationToken)
    {
        var mergerBackgroundService = scope.ServiceProvider.GetRequiredService<MergerBackgroundService>();

        return mergerBackgroundService.StartAsync(cancellationToken);
    }
}