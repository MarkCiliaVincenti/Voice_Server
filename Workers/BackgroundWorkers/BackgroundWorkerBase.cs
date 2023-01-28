namespace Workers.BackgroundWorkers;

/// <summary>
/// Base class that can be used to implement <see cref="IBackgroundWorker"/>.
/// </summary>
public abstract class BackgroundWorkerBase : IBackgroundWorker
{
    //TODO: Add UOW, Localization and other useful properties..?

    public IServiceProvider ServiceProvider { get; set; }
    protected CancellationTokenSource StoppingTokenSource { get; }
    protected CancellationToken StoppingToken { get; }

    public BackgroundWorkerBase()
    {
        StoppingTokenSource = new CancellationTokenSource();
        StoppingToken = StoppingTokenSource.Token;
    }

    public virtual Task StartAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public virtual Task StopAsync(CancellationToken cancellationToken = default)
    {
        StoppingTokenSource.Cancel();
        StoppingTokenSource.Dispose();
        return Task.CompletedTask;
    }

    public override string ToString()
    {
        return GetType().FullName;
    }
}