namespace ShardingStorage.Core.Web.Configurations;

public class DataAccessConfiguration
{
    public const string LsmTreeStorageTypeName = "lsm";
    public const string RedisStorageTypeName = "redis";

    public string StorageType { get; init; } = null!;
    public string ConfigurationPath { get; init; } = null!;
}