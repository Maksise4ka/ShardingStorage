using Microsoft.Extensions.DependencyInjection;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.BackgroundServices;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Configurations;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Repositories;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Services;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Services.Implementations;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLsmTreeStorage(
        this IServiceCollection services,
        LsmTreeStorageConfiguration configuration,
        FileServiceConfiguration fileServiceConfiguration)
    {
        Directory.CreateDirectory(fileServiceConfiguration.BaseDirectory);
        if (!File.Exists(configuration.WalPath))
        {
            using var _ = File.Create(configuration.WalPath);
        }

        services.AddSingleton<IWalService, WalService>();
        services.AddSingleton<ICompressorFactory, CompressorFactory>();
        services.AddSingleton<FileService>();

        services.AddSingleton<StorageItemRepositoryBase>();
        services.AddSingleton<IFileStorage>(x => x.GetRequiredService<FileService>());

        services.AddSingleton<MergerBackgroundService>();
        services.AddSingleton<IFileMerger>(x => x.GetRequiredService<FileService>());

        services.AddSingleton(configuration);
        services.AddSingleton(fileServiceConfiguration);

        return services;
    }
}