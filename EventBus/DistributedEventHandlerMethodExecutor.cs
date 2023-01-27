using Core;
using EventBus.Abstraction.EventBus;
using EventBus.Abstraction.EventBus.Distributed;

namespace EventBus;

public class DistributedEventHandlerMethodExecutor<TEvent> : IEventHandlerMethodExecutor
    where TEvent : class
{
    public EventHandlerMethodExecutorAsync ExecutorAsync => (target, parameter) =>
        target.As<IDistributedEventHandler<TEvent>>().HandleEventAsync(parameter.As<TEvent>());

    public Task ExecuteAsync(IEventHandler target, TEvent parameters)
    {
        return ExecutorAsync(target, parameters);
    }
}