using ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Wals;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Abstractions.Services;

// from replica to master
public interface IReplicaCommunicationService
{
    Task<Wal> Subscribe(string masterIp, string replicaIp, string replicaType);
}