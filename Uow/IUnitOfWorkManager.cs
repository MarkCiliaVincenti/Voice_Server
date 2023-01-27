using JetBrains.Annotations;

namespace Uow;

public interface IUnitOfWorkManager
{
    [CanBeNull]
    IUnitOfWork Current { get; }

    [NotNull]
    IUnitOfWork Begin([NotNull] EqnUnitOfWorkOptions options, bool requiresNew = false);

    [NotNull]
    IUnitOfWork Reserve([NotNull] string reservationName, bool requiresNew = false);

    void BeginReserved([NotNull] string reservationName, [NotNull] EqnUnitOfWorkOptions options);

    bool TryBeginReserved([NotNull] string reservationName, [NotNull] EqnUnitOfWorkOptions options);
}
