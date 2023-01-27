using Core;
using EventBus.Abstraction.EventBus;
using Volo.Eqn.EventBus;

namespace EventBus;

public class LocalEventHandlerMethodExecutor<TEvent> : IEventHandlerMethodExecutor
    where TEvent : class
{
    public EventHandlerMethodExecutorAsync ExecutorAsync => (target, parameter) =>
        target.As<ILocalEventHandler<TEvent>>().HandleEventAsync(parameter.As<TEvent>());

    public Task ExecuteAsync(IEventHandler target, TEvent parameters)
    {
        return ExecutorAsync(target, parameters);
    }
}