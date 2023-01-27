using JetBrains.Annotations;

namespace Core;

public class DisposeAction : IDisposable
{
    private readonly Action _action;

    public DisposeAction(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        _action = action;
    }

    public void Dispose()
    {
        _action();
    }
}

public class DisposeAction<T> : IDisposable
{
    private readonly Action<T> _action;

    [CanBeNull] private readonly T _parameter;

    public DisposeAction(Action<T> action, T parameter)
    {
        ArgumentNullException.ThrowIfNull(action);

        _action = action;
        _parameter = parameter;
    }

    public void Dispose()
    {
        _action(_parameter);
    }
}