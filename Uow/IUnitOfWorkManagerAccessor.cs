namespace Uow;

public interface IUnitOfWorkManagerAccessor
{
    IUnitOfWorkManager UnitOfWorkManager { get; }
}
