using ShardingStorage.Core.DataAccess.Abstractions.Repositories;

namespace ShardingStorage.Core.Application.Policies;

public interface IRatePolicy
{
    Task<ClientState> GetClientState(IDictionary<string, string> clientDescriptors, DateTime currentDate,
        IStorageItemRepository repository);

    Task UpdateRate(ClientState state, IStorageItemRepository repository);
}