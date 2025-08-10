using ShardingStorage.Orders.RateLimeter.Client.Clients;

namespace ShardingStorage.Orders.WebApi.Middlewares;

public class RateLimitingMiddleware : IMiddleware
{
    private readonly IRateLimeterClient _rateLimeterClient;

    public RateLimitingMiddleware(IRateLimeterClient rateLimeterClient)
    {
        _rateLimeterClient = rateLimeterClient;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var headers = context.Request.Headers
            .Select(x => x)
            .ToDictionary(x => x.Key, x => x.Value.ToString());

        var allowed = await _rateLimeterClient.IsAllowedAsync(headers, context.RequestAborted);

        if (allowed is false)
        {
            context.Response.StatusCode = 429;
            return;
        }

        await next.Invoke(context);
    }
}