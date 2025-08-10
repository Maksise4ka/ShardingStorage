using ShardingStorage.Core.Application.Models;
using ShardingStorage.Core.DataAccess.Abstractions.Repositories;

namespace ShardingStorage.Core.Application.Policies;

public class GeneralRatePolicy : RatePolicyBase
{
    private readonly Dictionary<string, ConcreteRatePolicy> _policies;

    public GeneralRatePolicy(DescriptorsConfiguration.RateLimit rateLimit, string key,
        ICollection<ConcreteRatePolicy> policies) : base(rateLimit, key)
    {
        _policies = policies.ToDictionary(p => p.Value, p => p);
    }

    public override async Task<ClientState> GetClientState(IDictionary<string, string> clientDescriptors,
        DateTime currentDate, IStorageItemRepository repository)
    {
        if (!clientDescriptors.TryGetValue(Key, out var value))
            return new ClientState(Key, null, currentDate, 0, false);

        if (_policies.TryGetValue(value, out var policy))
        {
            return await policy.GetClientState(clientDescriptors, currentDate, repository);
        }

        var (startDate, currentRate) = await GetCurrentRate(currentDate, Key, value, repository);

        return new ClientState(Key, value, startDate, currentRate, RateLimit.RequestsPerUnit >= currentRate);
    }
}