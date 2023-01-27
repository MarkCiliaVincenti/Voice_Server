namespace DistributedLockingAbstrcation;

public class LocalEqnDistributedLockHandle : IEqnDistributedLockHandle
{
    private readonly SemaphoreSlim _semaphore;

    public LocalEqnDistributedLockHandle(SemaphoreSlim semaphore)
    {
        _semaphore = semaphore;
    }

    public ValueTask DisposeAsync()
    {
        _semaphore.Release();
        return default;
    }
}
