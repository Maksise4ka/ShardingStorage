namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Services;

public interface ICompressor
{
    public Task Compress(Stream source, Stream destination, CancellationToken token);

    public Task<Stream> Decompress(Stream stream, CancellationToken token);
}