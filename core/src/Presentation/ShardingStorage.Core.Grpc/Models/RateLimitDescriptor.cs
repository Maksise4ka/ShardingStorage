namespace ShardingStorage.Core.Grpc.Models;

public class RateLimitDescriptor
{
    public required string Key { get; init; }
    
    public required string? Value { get; init; }
    
    public required string Unit { get; init; }
    
    public required int RequestsPerUnit { get; init; }
}