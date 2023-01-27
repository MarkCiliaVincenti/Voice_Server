namespace Uow;

public interface IAmbientUnitOfWork : IUnitOfWorkAccessor
{
    IUnitOfWork GetCurrentByChecking();
}
