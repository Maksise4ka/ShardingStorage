using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Configurations;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Configurations;
using ShardingStorage.Core.Storage.Web.Exceptions;

namespace ShardingStorage.Core.Storage.Web.Configurations;

public class DataAccessConfiguration
{
    public const string LsmTreeStorageTypeName = "lsm-tree";
    
    public string StorageType { get; init; } = null!;
    
    public LsmTreeStorageConfiguration LsmTreeStorageConfiguration { get; init; } = null!;
    public FileServiceConfiguration FileServiceConfiguration { get; init; } = null!;
    
    public NodeConfiguration NodeConfiguration { get; init; } = null!;
    
    public static DataAccessConfiguration Parse(IConfiguration configuration)
    {
        var result = configuration.Get<DataAccessConfiguration>();

        if (result is null)
            throw new InvalidConfigurationException();

        return result;
    }
}