namespace Uow;

public class AlwaysDisableTransactionsUnitOfWorkManager : IUnitOfWorkManager
{
    private readonly UnitOfWorkManager _unitOfWorkManager;

    public AlwaysDisableTransactionsUnitOfWorkManager(UnitOfWorkManager unitOfWorkManager)
    {
        _unitOfWorkManager = unitOfWorkManager;
    }

    public IUnitOfWork Current => _unitOfWorkManager.Current;

    public IUnitOfWork Begin(EqnUnitOfWorkOptions options, bool requiresNew = false)
    {
        options.IsTransactional = false;
        return _unitOfWorkManager.Begin(options, requiresNew);
    }

    public IUnitOfWork Reserve(string reservationName, bool requiresNew = false)
    {
        return _unitOfWorkManager.Reserve(reservationName, requiresNew);
    }

    public void BeginReserved(string reservationName, EqnUnitOfWorkOptions options)
    {
        options.IsTransactional = false;
        _unitOfWorkManager.BeginReserved(reservationName, options);
    }

    public bool TryBeginReserved(string reservationName, EqnUnitOfWorkOptions options)
    {
        options.IsTransactional = false;
        return _unitOfWorkManager.TryBeginReserved(reservationName, options);
    }
}