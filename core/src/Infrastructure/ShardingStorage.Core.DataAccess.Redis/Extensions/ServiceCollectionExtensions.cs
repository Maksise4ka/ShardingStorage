using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ShardingStorage.Core.DataAccess.Abstractions.Contexts;
using ShardingStorage.Core.DataAccess.Abstractions.Repositories;
using ShardingStorage.Core.DataAccess.Common.Configurations;
using ShardingStorage.Core.DataAccess.Common.Contexts;
using ShardingStorage.Core.DataAccess.Common.Hash;
using ShardingStorage.Core.DataAccess.Redis.Configurations;
using ShardingStorage.Core.DataAccess.Redis.Repositories;
using StackExchange.Redis;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ShardingStorage.Core.DataAccess.Redis.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddShardingRedis(this IServiceCollection services, string configurationPath)
    {
        services.ConfigureShard(configurationPath);
        services.AddScoped<IApplicationContext, ApplicationContext>();
        services.AddScoped<IStorageItemRepository, StorageItemRepository>();

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
        services.AddScoped<IHashStrategy, Md5HashStrategy>();

        services.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(
            _ => ConnectionMultiplexer.Connect(CreateRedisConnectionString(masters)));

        return services;
    }

    private static string CreateRedisConnectionString(IList<MasterConfiguration> masters)
    {
        StringBuilder builder = new();
        foreach (var master in masters)
        {
            builder.Append($"{master.Master},password={master.Password},");
            foreach (var replica in master.Replicas)
            {
                builder.Append($"{replica.Replica},password={replica.Password},");
            }
        }

        return builder.ToString()[..^1];
    }

}