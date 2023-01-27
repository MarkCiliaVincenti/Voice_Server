using Microsoft.Extensions.Options;

namespace DistributedLockingAbstrcation;

public class DistributedLockKeyNormalizer : IDistributedLockKeyNormalizer
{
    protected EqnDistributedLockOptions Options { get; }
    
    public DistributedLockKeyNormalizer(IOptions<EqnDistributedLockOptions> options)
    {
        Options = options.Value;
    }
    
    public virtual string NormalizeKey(string name)
    {
        return $"{Options.KeyPrefix}{name}";
    }
}