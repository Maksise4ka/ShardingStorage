namespace ShardingStorage.Core.Application.Policies;

public record ClientState(String Key, string? Value, DateTime StartDate, int CurrentRequests, bool Allowed);
