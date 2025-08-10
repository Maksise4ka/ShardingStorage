using ShardingStorage.Orders.Client.Models;

namespace ShardingStorage.Orders.Client.Requests;

public record CreateOrderRequest(List<OrderItem> Items);