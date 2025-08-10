namespace ShardingStorage.Orders.RateLimeter.Client.Clients;

public class LocalRateLimiter : IRateLimeterClient
{
    private int _counter = 10;
    
    public Task<bool> IsAllowedAsync(Dictionary<string, string> headers, CancellationToken cancellationToken = default)
    {
        --_counter;

        return Task.FromResult(_counter >= 0);
    }
}