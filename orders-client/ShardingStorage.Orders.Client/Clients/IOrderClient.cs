using ShardingStorage.Orders.Client.Models;
using ShardingStorage.Orders.Client.Requests;

namespace ShardingStorage.Orders.Client.Clients;

public interface IOrderClient
{
    Order Create(CreateOrderRequest request);

    IReadOnlyCollection<Order> Get();
}