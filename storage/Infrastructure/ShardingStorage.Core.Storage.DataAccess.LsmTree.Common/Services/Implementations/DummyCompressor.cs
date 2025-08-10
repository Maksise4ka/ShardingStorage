namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Services.Implementations;

public class DummyCompressor : ICompressor
{
    public async Task Compress(Stream source, Stream destination, CancellationToken token)
    {
        await source.CopyToAsync(destination, token);
    }

    public async Task<Stream> Decompress(Stream stream, CancellationToken token)
    {
        var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, token);
        memoryStream.Seek(0, SeekOrigin.Begin);

        return memoryStream;
    }
}