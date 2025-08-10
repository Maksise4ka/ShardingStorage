using Grpc.Core;
using Microsoft.Extensions.Logging;
using ShardingStorage.Core.Storage.Data.Access.LsmTree.Grpc;
using ShardingStorage.Core.Storage.DataAccess.LsmTree.Abstractions.Repositories;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Listeners;

public class MasterListener : Data.Access.LsmTree.Grpc.MasterService.MasterServiceBase
{
    private readonly IMasterRepository _repository;
    private readonly ILogger<MasterListener> _logger;

    public MasterListener(IMasterRepository repository, ILogger<MasterListener> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public override Task<SubscribeResponse> Subscribe(SubscribeRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"asked to subscribe from {request.ReplicaIp}");

        var wal = _repository.Subscribe(request.ReplicaIp, request.ReplicaType.ToString());
        var arr = wal.History.Select(x => new StorageItem() { Key = x.Key, Value = x.Value }).ToArray();

        return Task.FromResult(new SubscribeResponse {Items = {arr} });
    }
}