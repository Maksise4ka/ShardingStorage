namespace ShardingStorage.Core.Storage.Application.Contracts.Services;

public interface IStorageService
{
    Task SetAsync(string key, string value, CancellationToken cancellationToken = default);

    Task<string?> GetAsync(string key, CancellationToken cancellationToken = default);
}