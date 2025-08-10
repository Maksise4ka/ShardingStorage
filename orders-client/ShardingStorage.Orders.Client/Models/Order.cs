using System.Text.Json.Serialization;

namespace ShardingStorage.Orders.Client.Models;

public class Order
{
    [JsonPropertyName("id")]
    public required long Id { get; set; }
    
    [JsonPropertyName("items")]
    public required List<OrderItem> Items { get; set; }
}