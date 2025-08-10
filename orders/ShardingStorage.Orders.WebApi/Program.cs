using ShardingStorage.Orders.RateLimeter.Client.Extensions;
using ShardingStorage.Orders.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var rateLimiterAddress = Environment.GetEnvironmentVariable("RATE_LIMITER_ADDRESS");
builder.Services.AddRateLimiterClient(rateLimiterAddress);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<RateLimitingMiddleware>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<RateLimitingMiddleware>();

app.MapControllers();

app.Run();