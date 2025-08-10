using Microsoft.Extensions.DependencyInjection;
using ShardingStorage.Core.Storage.Application.Contracts.Services;
using ShardingStorage.Core.Storage.Application.Services;

namespace ShardingStorage.Core.Storage.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IStorageService, StorageService>();

        return services;
    }
}