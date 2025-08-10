using Microsoft.Extensions.DependencyInjection;
using ShardingStorage.Core.DataAccess.Abstractions.Contexts;
using ShardingStorage.Core.DataAccess.Abstractions.Repositories;
using ShardingStorage.Core.DataAccess.Common.Configurations;
using ShardingStorage.Core.DataAccess.Common.Contexts;
using ShardingStorage.Core.DataAccess.Common.Hash;
using ShardingStorage.Core.DataAccess.Lsm.Configurations;
using ShardingStorage.Core.DataAccess.Lsm.Repositories;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ShardingStorage.Core.DataAccess.Lsm.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddShardingLsm(this IServiceCollection services, string configurationPath)
    {
        services.ConfigureShard(configurationPath);
        services.AddScoped<IApplicationContext, ApplicationContext>();
        services.AddScoped<IStorageItemRepository, StorageItemRepository>();
        services.AddScoped<IHashStrategy, Md5HashStrategy>();

        return services;
    }

    private static IServiceCollection ConfigureShard(this IServiceCollection services, string configurationPath)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        using var sr = File.OpenText(configurationPath);
        var masters = deserializer.Deserialize<List<MasterConfiguration>>(sr);
        
        services.AddSingleton<NodeConfiguration>(_ => new NodeConfiguration(
            masters.Select(m => m.AsNode()).ToList()));

        return services;
    }
}