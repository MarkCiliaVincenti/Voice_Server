namespace Uow;

public class NullUnitOfWorkTransactionBehaviourProvider : IUnitOfWorkTransactionBehaviourProvider
{
    public bool? IsTransactional => null;
}
