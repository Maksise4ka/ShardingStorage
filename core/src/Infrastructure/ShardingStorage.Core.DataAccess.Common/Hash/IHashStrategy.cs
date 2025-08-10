namespace ShardingStorage.Core.DataAccess.Common.Hash;

public interface IHashStrategy
{
    int ComputeHash(string s);
}