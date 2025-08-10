using ShardingStorage.Core.Storage.Web.Configurations;
using ShardingStorage.Core.Storage.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = DataAccessConfiguration.Parse(builder.Configuration);

builder.Services.Configure(configuration);

var application = builder.Build();

application.Configure();

application.InitializeNode(configuration.NodeConfiguration);
await application.InitializeAsync(configuration);

await application.RunAsync();