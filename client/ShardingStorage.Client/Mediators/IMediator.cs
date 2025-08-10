using ShardingStorage.Client.Handlers;

namespace ShardingStorage.Client.Mediators;

public interface IMediator
{
    IMediator AddHandler(string command, IHandler handler);
    void Handle(string command, string[] args);
}