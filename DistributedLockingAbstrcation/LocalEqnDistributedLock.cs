using System.Collections.Concurrent;
using Core;

namespace DistributedLockingAbstrcation;

public class LocalEqnDistributedLock : IEqnDistributedLock
{
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _localSyncObjects = new();
    protected IDistributedLockKeyNormalizer DistributedLockKeyNormalizer { get; }

    public LocalEqnDistributedLock(IDistributedLockKeyNormalizer distributedLockKeyNormalizer)
    {
        DistributedLockKeyNormalizer = distributedLockKeyNormalizer;
    }

    public async Task<IEqnDistributedLockHandle> TryAcquireAsync(
        string name,
        TimeSpan timeout = default,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        var key = DistributedLockKeyNormalizer.NormalizeKey(name);

        var semaphore = _localSyncObjects.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));

        if (!await semaphore.WaitAsync(timeout, cancellationToken))
        {
            return null;
        }

        return new LocalEqnDistributedLockHandle(semaphore);
    }
}