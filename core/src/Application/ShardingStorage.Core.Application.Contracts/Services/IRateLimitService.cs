namespace ShardingStorage.Core.Application.Contracts.Services;

public interface IRateLimitService
{
    Task<bool> Update(Dictionary<string, string> clientDescriptors, CancellationToken token);
}