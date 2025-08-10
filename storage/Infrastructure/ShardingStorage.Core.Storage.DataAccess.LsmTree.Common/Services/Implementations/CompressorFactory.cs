using System.IO.Compression;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Services.Implementations;

public class CompressorFactory : ICompressorFactory
{
    public ICompressor GetCompressor(string compressorName)
    {
        ICompressor compressor = compressorName switch
        {
            "GZipCompressor:Fastest" => new GZipCompressor(CompressionLevel.Fastest),
            "GZipCompressor:Optimal" => new GZipCompressor(CompressionLevel.Optimal),
            "GZipCompressor:SmallestSize" => new GZipCompressor(CompressionLevel.SmallestSize),
            "NoCompression" => new DummyCompressor(),
            _ => throw new Exception($"{compressorName} is not valid")
        };

        return compressor;
    }

    public string NameOfCompressor(ICompressor compressor)
    {
        var result = compressor switch
        {
            GZipCompressor gzip => gzip.Level switch
            {
                CompressionLevel.Fastest => "GZipCompressor:Fastest",
                CompressionLevel.Optimal => "GZipCompressor:Optimal",
                CompressionLevel.SmallestSize => "GZipCompressor:SmallestSize",
                _ => throw new Exception($"GZipCompressor with {gzip.Level} is not valid")
            },
            DummyCompressor => "NoCompression",
            _ => throw new Exception($"{compressor} is not supported")
        };

        return result;
    }
}