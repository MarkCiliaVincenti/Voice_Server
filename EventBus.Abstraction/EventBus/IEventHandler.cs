using EventBus.Abstraction.EventBus.Distributed;
using Volo.Eqn.EventBus;

namespace EventBus.Abstraction.EventBus;

/// <summary>
/// Indirect base interface for all event handlers.
/// Implement <see cref="ILocalEventHandler{TEvent}"/> or <see cref="IDistributedEventHandler{TEvent}"/> instead of this one.
/// </summary>
public interface IEventHandler
{

}
