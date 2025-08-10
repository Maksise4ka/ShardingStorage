namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Services;

internal interface IFileMerger
{
    Task MergeAsync(CancellationToken cancellationToken = default);
}