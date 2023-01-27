using Core;
using DistributedLockingAbstrcation;
using Medallion.Threading;

namespace DistributedLocking;

public static class EqnDistributedLockHandleExtensions
{
    public static IDistributedSynchronizationHandle ToDistributedSynchronizationHandle(
        this IEqnDistributedLockHandle handle)
    {
        return handle.As<MedallionEqnDistributedLockHandle>().Handle;
    }
}
