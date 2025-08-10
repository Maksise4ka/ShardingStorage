namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Services;

public interface ICompressorFactory
{
    ICompressor GetCompressor(string compressorName);

    string NameOfCompressor(ICompressor compressor);
}