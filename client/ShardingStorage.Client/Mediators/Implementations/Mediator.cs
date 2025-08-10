using ShardingStorage.Client.Exceptions;
using ShardingStorage.Client.Handlers;

namespace ShardingStorage.Client.Mediators.Implementations;

public class Mediator : IMediator
{
    private readonly Dictionary<string, IHandler> _handlers;

    public Mediator()
    {
        _handlers = new Dictionary<string, IHandler>();
    }

    public IMediator AddHandler(string command, IHandler handler)
    {
        if (_handlers.ContainsKey(command))
            throw MediatorException.CommandAlreadyExists(command);

        _handlers.Add(command, handler);

        return this;
    }

    public void Handle(string command, string[] args)
    {
        if (!_handlers.ContainsKey(command))
            throw MediatorException.CommandNotExists(command);

        _handlers[command].Handle(args);
    }
}