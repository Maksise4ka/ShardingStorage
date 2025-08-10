using System.Text.Json.Serialization;

namespace ShardingStorage.Orders.Client.Models;

public class OrderItem
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("price")]
    public decimal Price { get; init; }
    
    [JsonPropertyName("count")]
    public int Count { get; init; }
}