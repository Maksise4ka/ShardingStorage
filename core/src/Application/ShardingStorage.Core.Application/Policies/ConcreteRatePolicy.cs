using ShardingStorage.Core.Application.Models;
using ShardingStorage.Core.DataAccess.Abstractions.Repositories;

namespace ShardingStorage.Core.Application.Policies;

public class ConcreteRatePolicy : RatePolicyBase
{
    public ConcreteRatePolicy(DescriptorsConfiguration.RateLimit rateLimit, string key, string value) : base(rateLimit, key)
    {
        Value = value;
    }

    public string Value { get; }

    public override async Task<ClientState> GetClientState(IDictionary<string, string> clientDescriptors, DateTime currentDate, IStorageItemRepository repository)
    {
        if (!clientDescriptors.TryGetValue(Key, out var value) || value != Value)
            return new ClientState(Key, null, currentDate, 0, false);

        var (startDate, currentRate) = await GetCurrentRate(currentDate, Key, Value, repository);

        return new ClientState(Key, Value, startDate, currentRate,RateLimit.RequestsPerUnit >= currentRate);
    }
}