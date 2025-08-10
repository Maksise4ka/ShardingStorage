namespace ShardingStorage.Orders.WebApi.Models;

public class OrderItem
{
    public required string Name { get; set; }
    
    public required decimal Price { get; init; }
    
    public required int Count { get; init; }
}