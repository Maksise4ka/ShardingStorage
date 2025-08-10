namespace ShardingStorage.Orders.WebApi.Models;

public class Order
{
    public required long Id { get; set; }
    
    public required List<OrderItem> Items { get; set; }
}