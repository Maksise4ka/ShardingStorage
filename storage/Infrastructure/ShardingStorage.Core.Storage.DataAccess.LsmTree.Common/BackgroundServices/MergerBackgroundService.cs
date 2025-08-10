using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Configurations;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Services;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.BackgroundServices;

internal sealed class MergerBackgroundService : BackgroundService
{
    private readonly LsmTreeStorageConfiguration _configuration;
    private readonly IFileMerger _fileMerger;
    private readonly ILogger<MergerBackgroundService> _logger;

    public MergerBackgroundService(
        IFileMerger fileMerger,
        LsmTreeStorageConfiguration configuration,
        ILogger<MergerBackgroundService> logger)
    {
        _fileMerger = fileMerger;
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(_configuration.FirstMergingMillisecondsDelay, stoppingToken);
        
        while (stoppingToken.IsCancellationRequested is false)
        {
            _logger.LogInformation("File merging has started");

            await _fileMerger.MergeAsync(stoppingToken);

            _logger.LogInformation("File merging finished");

            await Task.Delay(_configuration.MergingMillisecondsDelay, stoppingToken);
        }
    }
}