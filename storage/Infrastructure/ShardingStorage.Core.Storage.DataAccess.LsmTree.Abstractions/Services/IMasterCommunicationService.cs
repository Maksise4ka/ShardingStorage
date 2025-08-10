using ShardingStorage.Core.Storage.Domain.Entities;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Abstractions.Services;

// from master to replica
public interface IMasterCommunicationService
{
    void Append(string replicaIp, StorageItem storageItem);
    Task AppendAsync(string replicaIp, StorageItem storageItem);
}