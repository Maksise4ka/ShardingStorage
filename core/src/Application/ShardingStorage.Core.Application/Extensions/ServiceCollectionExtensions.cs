using Microsoft.Extensions.DependencyInjection;
using ShardingStorage.Core.Application.Contracts.Services;
using ShardingStorage.Core.Application.Models;
using ShardingStorage.Core.Application.Policies;
using ShardingStorage.Core.Application.Services;

namespace ShardingStorage.Core.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, string descriptorPath)
    {
        services.AddScoped<IRateLimitService, RateLimitService>();

        var configuration = DescriptorsConfiguration.FromYamlConfiguration(descriptorPath);
        var policies = GetPolicies(configuration);
        services.AddSingleton<IEnumerable<IRatePolicy>>(_ => policies);

        return services;
    }

    private static IEnumerable<IRatePolicy> GetPolicies(DescriptorsConfiguration configuration)
    {
        Dictionary<string, List<ConcreteRatePolicy>> concretePolicies = new();
        List<GeneralRatePolicy> policies = new();

        foreach (var descriptor in configuration.Descriptors)
        {
            if (descriptor.Value is null)
                continue;

            if (!concretePolicies.TryGetValue(descriptor.Key, out var list))
            {
                list = new List<ConcreteRatePolicy>();
                concretePolicies[descriptor.Key] = list;
            }

            list.Add(new ConcreteRatePolicy(descriptor.RateLimit, descriptor.Key, descriptor.Value));
        }

        foreach (var descriptor in configuration.Descriptors)
        {
            if (descriptor.Value is not null)
                continue;

            if (!concretePolicies.TryGetValue(descriptor.Key, out var list))
                list = new List<ConcreteRatePolicy>();
            else
                concretePolicies.Remove(descriptor.Key);

            var policy = new GeneralRatePolicy(descriptor.RateLimit, descriptor.Key, list);
            policies.Add(policy);
        }

        var remainPolicies = concretePolicies.Select(kvp =>
            new GeneralRatePolicy(
                new DescriptorsConfiguration.RateLimit
                {
                    RequestsPerUnit = int.MaxValue,
                    Unit = DescriptorsConfiguration.Unit.Hour
                }, 
            kvp.Key, 
            kvp.Value)).ToList();
        policies.AddRange(remainPolicies);
        return policies;
    }
}