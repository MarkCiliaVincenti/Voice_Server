using JetBrains.Annotations;

namespace Uow;

public interface IUnitOfWorkAccessor
{
    [CanBeNull]
    IUnitOfWork UnitOfWork { get; }

    void SetUnitOfWork([CanBeNull] IUnitOfWork unitOfWork);
}
