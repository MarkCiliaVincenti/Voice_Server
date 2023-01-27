using System.Data;

namespace Uow;

public class EqnUnitOfWorkOptions : IEqnUnitOfWorkOptions
{
    /// <summary>
    /// Default: false.
    /// </summary>
    public bool IsTransactional { get; set; }

    public IsolationLevel? IsolationLevel { get; set; }

    /// <summary>
    /// Milliseconds
    /// </summary>
    public int? Timeout { get; set; }

    public EqnUnitOfWorkOptions()
    {

    }

    public EqnUnitOfWorkOptions(bool isTransactional = false, IsolationLevel? isolationLevel = null, int? timeout = null)
    {
        IsTransactional = isTransactional;
        IsolationLevel = isolationLevel;
        Timeout = timeout;
    }

    public EqnUnitOfWorkOptions Clone()
    {
        return new EqnUnitOfWorkOptions
        {
            IsTransactional = IsTransactional,
            IsolationLevel = IsolationLevel,
            Timeout = Timeout
        };
    }
}
