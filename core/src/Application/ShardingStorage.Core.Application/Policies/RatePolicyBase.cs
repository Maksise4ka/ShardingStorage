using ShardingStorage.Core.Application.Models;
using ShardingStorage.Core.DataAccess.Abstractions.Repositories;
using ShardingStorage.Core.Domain.Entities;

namespace ShardingStorage.Core.Application.Policies;


public abstract class RatePolicyBase : IRatePolicy
{
    protected RatePolicyBase(DescriptorsConfiguration.RateLimit rateLimit, string key)
    {
        RateLimit = rateLimit;
        Key = key;
    }

    protected string Key { get; }
    protected DescriptorsConfiguration.RateLimit RateLimit { get; }

    protected record CurrentRate(DateTime Time, int Requests);
    protected static CurrentRate ParseValue(string value)
    {
        var split = value.Split("#");
        return new CurrentRate(DateTime.Parse(split[0]), int.Parse(split[1]));
    }
    
    protected async Task<CurrentRate> GetCurrentRate(DateTime currentDate, string key, string clientValue, IStorageItemRepository repository)
    {
        var item = await repository.FindByKeyAsync(ToStorageKey(key, clientValue));
        
        int currentRate = 1;
        DateTime startDate = currentDate;
        if (item is null)
            return new CurrentRate(startDate, currentRate);

        var storageRate = ParseValue(item.Value);
        TimeSpan span = currentDate - storageRate.Time;
        if (span >= RateLimit.ToTimeSpan())
        {
            currentRate = 1;
            startDate = DateTime.Now;
        }
        else
            currentRate = storageRate.Requests + 1;

        return new CurrentRate(startDate, currentRate);
    }

    private static string RateToString(DateTime time, int requests) => $"{time}#{requests}";
    private static string ToStorageKey(string key, string value) => $"{key}:{value}";
    
    public abstract Task<ClientState> GetClientState(IDictionary<string, string> clientDescriptors, DateTime currentDate, IStorageItemRepository repository);

    public async Task UpdateRate(ClientState state, IStorageItemRepository repository)
    {
        ArgumentNullException.ThrowIfNull(state.Value);

        var storageKey = ToStorageKey(state.Key, state.Value);
        var storageValue = RateToString(state.StartDate, state.CurrentRequests);

        await repository.Add(new StorageItem(storageKey, storageValue));
    }
}