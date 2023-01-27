using EventBus.Abstraction.EventBus;

namespace EventBus;

public delegate Task EventHandlerMethodExecutorAsync(IEventHandler target, object parameter);