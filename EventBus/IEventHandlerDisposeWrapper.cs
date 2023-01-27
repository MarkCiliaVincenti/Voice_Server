using EventBus.Abstraction.EventBus;

namespace EventBus;

public interface IEventHandlerDisposeWrapper : IDisposable
{
    IEventHandler EventHandler { get; }
}
