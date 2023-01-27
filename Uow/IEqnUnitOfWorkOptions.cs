using System.Data;

namespace Uow;

public interface IEqnUnitOfWorkOptions
{
    bool IsTransactional { get; }

    IsolationLevel? IsolationLevel { get; }

    /// <summary>
    /// Milliseconds
    /// </summary>
    int? Timeout { get; }
}
