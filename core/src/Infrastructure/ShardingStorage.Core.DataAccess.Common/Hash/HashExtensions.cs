namespace ShardingStorage.Core.DataAccess.Common.Hash;

public static class HashExtensions
{
    public static int ComputeHash(this IHashStrategy strategy, string s, int maxValue, int offset = 0)
        => Math.Abs(strategy.ComputeHash(s)) % maxValue + offset;
}