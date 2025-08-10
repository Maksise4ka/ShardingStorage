using ShardingStorage.Core.Web.Configurations;
using ShardingStorage.Core.Web.Exceptions;
using ShardingStorage.Core.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

var dataAccess = builder.Configuration
                     .GetSection("DataAccess")
                     .Get<DataAccessConfiguration>()
                 ?? throw new InvalidConfigurationException();

var rateLimit = builder.Configuration
                     .GetSection("RateLimiter")
                     .Get<RateLimiterConfiguration>()
                 ?? throw new InvalidConfigurationException();


builder.Services.Configure(dataAccess, rateLimit);

var application = builder.Build();

application.Configure();

await application.InitializeAsync();

await application.RunAsync();