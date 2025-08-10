using Grpc.Core;
using Microsoft.Extensions.Logging;
using ShardingStorage.Core.Storage.Data.Access.LsmTree.Grpc;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Abstractions.Repositories;
using StorageItem = ShardingStorage.Core.Storage.Domain.Entities.StorageItem;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Listeners;

public class ReplicaListener : ReplicaService.ReplicaServiceBase
{
    private readonly IReplicaRepository _replicaRepository;
    private readonly ILogger<ReplicaListener> _logger;

    public ReplicaListener(IReplicaRepository replicaRepository, ILogger<ReplicaListener> logger)
    {
        _replicaRepository = replicaRepository;
        _logger = logger;
    }

    public override Task<AppendResponse> Append(AppendRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"asked to append {request.Item.Key}:{request.Item.Value}");

        _replicaRepository.Append(new StorageItem(request.Item.Key, request.Item.Value));

        return Task.FromResult(new AppendResponse());
    }
}