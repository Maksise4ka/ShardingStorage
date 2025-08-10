using ShardingStorage.Grpc.Contracts;
using static ShardingStorage.Grpc.Contracts.RateLimiterService;

namespace ShardingStorage.Orders.RateLimeter.Client.Clients;

public class RateLimiterClient : IRateLimeterClient
{
    private readonly RateLimiterServiceClient _client;

    public RateLimiterClient(RateLimiterServiceClient client)
    {
        _client = client;
    }

    public async Task<bool> IsAllowedAsync(
        Dictionary<string, string> headers,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateRequest
        {
            Descriptors =
            {
                headers.Select(x => new RateLimitDescriptor
                {
                    Key = x.Key,
                    Value = x.Value
                })
            }
        };

        var response = await _client.UpdateAsync(request, cancellationToken: cancellationToken);

        return response.Allowed;
    }
}