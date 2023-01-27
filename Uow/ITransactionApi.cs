namespace Uow;

public interface ITransactionApi : IDisposable
{
    Task CommitAsync();
}
