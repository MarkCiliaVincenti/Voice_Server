namespace DistributedLockingAbstrcation;

public class EqnDistributedLockOptions
{
    /// <summary>
    /// DistributedLock key prefix.
    /// </summary>
    public string KeyPrefix  { get; set; }
    
    public EqnDistributedLockOptions()
    {
        KeyPrefix = "";
    }
}