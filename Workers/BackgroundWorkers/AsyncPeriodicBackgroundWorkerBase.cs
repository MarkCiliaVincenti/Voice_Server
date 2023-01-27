using Core;
using Core.ExceptionHandling;
using Microsoft.Extensions.DependencyInjection;
using Threading;

namespace Workers.BackgroundWorkers;

public abstract class AsyncPeriodicBackgroundWorkerBase : BackgroundWorkerBase
{
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected EqnAsyncTimer Timer { get; }
    protected CancellationToken StartCancellationToken { get; set; }

    protected AsyncPeriodicBackgroundWorkerBase(
        EqnAsyncTimer timer,
        IServiceScopeFactory serviceScopeFactory)
    {
        ServiceScopeFactory = serviceScopeFactory;
        Timer = timer;
        Timer.Elapsed = Timer_Elapsed;
    }

    public async override Task StartAsync(CancellationToken cancellationToken = default)
    {
        StartCancellationToken = cancellationToken;

        await base.StartAsync(cancellationToken);
        Timer.Start(cancellationToken);
    }

    public async override Task StopAsync(CancellationToken cancellationToken = default)
    {
        Timer.Stop(cancellationToken);
        await base.StopAsync(cancellationToken);
    }

    private async Task Timer_Elapsed(EqnAsyncTimer timer)
    {
        await DoWorkAsync(StartCancellationToken);
    }

    private async Task DoWorkAsync(CancellationToken cancellationToken = default)
    {
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            try
            {
                await DoWorkAsync(new PeriodicBackgroundWorkerContext(scope.ServiceProvider, cancellationToken));
            }
            catch (Exception ex)
            {
                await scope.ServiceProvider
                    .GetRequiredService<IExceptionNotifier>()
                    .NotifyAsync(new ExceptionNotificationContext(ex));
            }
        }
    }

    protected abstract Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext);
}