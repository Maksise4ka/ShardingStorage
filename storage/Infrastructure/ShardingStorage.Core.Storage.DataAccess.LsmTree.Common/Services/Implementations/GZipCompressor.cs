using System.IO.Compression;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Services.Implementations;

public class GZipCompressor : ICompressor
{
    public GZipCompressor(CompressionLevel level)
    {
        Level = level;
    }

    public CompressionLevel Level { get; }

    public async Task Compress(Stream source, Stream destination, CancellationToken token)
    {
        await using var gzip = new GZipStream(destination, Level, true);
        await source.CopyToAsync(gzip, token);
    }

    public async Task<Stream> Decompress(Stream stream, CancellationToken token)
    {
        await using var decompressedStream = new GZipStream(stream, CompressionMode.Decompress, true);
        var memoryStream = new MemoryStream();

        await decompressedStream.CopyToAsync(memoryStream, token);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }
}