using ShardingStorage.Orders.WebApi.Models;

namespace ShardingStorage.Orders.WebApi.Requests;

public record CreateOrderRequest(List<OrderItem> Items);