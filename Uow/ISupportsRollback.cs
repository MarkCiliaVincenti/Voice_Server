namespace Uow;

public interface ISupportsRollback
{
    Task RollbackAsync(CancellationToken cancellationToken);
}
