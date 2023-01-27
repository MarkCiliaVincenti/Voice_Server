namespace Uow;

public interface IUnitOfWorkTransactionBehaviourProvider
{
    bool? IsTransactional { get; }
}
