using ShardingStorage.Core.Application.Contracts.Services;
using ShardingStorage.Core.Application.Policies;
using ShardingStorage.Core.DataAccess.Abstractions.Repositories;

namespace ShardingStorage.Core.Application.Services;

public class RateLimitService : IRateLimitService
{
    private readonly IList<IRatePolicy> _policies;
    private readonly IStorageItemRepository _repository;

    public RateLimitService(IEnumerable<IRatePolicy> policies, IStorageItemRepository repository)
    {
        _repository = repository;
        _policies = policies.ToList();
    }

    public async Task<bool> Update(Dictionary<string, string> clientDescriptors, CancellationToken token)
    {
        var time = DateTime.Now;
        List<ClientState> states = new();

        foreach (var policy in _policies)
        {
            var state = await policy.GetClientState(clientDescriptors, time, _repository);
            if (!state.Allowed)
                return false;

            states.Add(state);
        }

        for (int i = 0; i < _policies.Count; ++i)
        {
            await _policies[i].UpdateRate(states[i], _repository);
        }

        return true;
    }

}