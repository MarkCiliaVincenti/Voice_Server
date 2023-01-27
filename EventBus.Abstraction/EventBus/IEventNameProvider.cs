namespace EventBus.Abstraction.EventBus;

public interface IEventNameProvider
{
    string GetName(Type eventType);
}
