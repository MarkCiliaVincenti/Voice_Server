namespace DistributedLockingAbstrcation;

public interface IDistributedLockKeyNormalizer
{
    string NormalizeKey(string name);
    
}