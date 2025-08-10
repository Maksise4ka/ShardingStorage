namespace ShardingStorage.Orders.RateLimeter.Client.Clients;

public interface IRateLimeterClient
{
    Task<bool> IsAllowedAsync(Dictionary<string, string> headers, CancellationToken cancellationToken = default);
}