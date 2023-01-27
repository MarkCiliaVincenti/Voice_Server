using DistributedLockingAbstrcation;
using Medallion.Threading;

namespace DistributedLocking;

public class MedallionEqnDistributedLockHandle : IEqnDistributedLockHandle
{
    public IDistributedSynchronizationHandle Handle { get; }

    public MedallionEqnDistributedLockHandle(IDistributedSynchronizationHandle handle)
    {
        Handle = handle;
    }

    public ValueTask DisposeAsync()
    {
        return Handle.DisposeAsync();
    }
}
