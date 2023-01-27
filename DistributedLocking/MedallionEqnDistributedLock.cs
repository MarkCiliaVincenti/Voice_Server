using Core;
using DistributedLockingAbstrcation;
using Medallion.Threading;

namespace DistributedLocking;
public class MedallionEqnDistributedLock : IEqnDistributedLock
{
    protected IDistributedLockProvider DistributedLockProvider { get; }

    protected IDistributedLockKeyNormalizer DistributedLockKeyNormalizer { get; }

    public MedallionEqnDistributedLock(
        IDistributedLockProvider distributedLockProvider,
        IDistributedLockKeyNormalizer distributedLockKeyNormalizer)
    {
        DistributedLockProvider = distributedLockProvider;
        DistributedLockKeyNormalizer = distributedLockKeyNormalizer;
    }

    public async Task<IEqnDistributedLockHandle> TryAcquireAsync(
        string name,
        TimeSpan timeout = default,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        var key = DistributedLockKeyNormalizer.NormalizeKey(name);
        
       var token = new CancellationToken();

        var handle = await DistributedLockProvider.TryAcquireLockAsync(
            key,
            timeout,
            token
        );
        
        if (handle == null)
        {
            return null;
        }

        return new MedallionEqnDistributedLockHandle(handle);
    }
}
