using Core;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Workers.BackgroundWorkers;

public static class BackgroundWorkersApplicationInitializationContextExtensions
{
    public async static Task AddBackgroundWorkerAsync<TWorker>(
        [NotNull] this IServiceProvider context, CancellationToken cancellationToken = default)
        where TWorker : IBackgroundWorker
    {
        Check.NotNull(context, nameof(context));

        await context.AddBackgroundWorkerAsync(typeof(TWorker), cancellationToken: cancellationToken);
    }

    public async static Task AddBackgroundWorkerAsync([NotNull] this IServiceProvider context,
        [NotNull] Type workerType, CancellationToken cancellationToken = default)
    {
        Check.NotNull(context, nameof(context));
        Check.NotNull(workerType, nameof(workerType));

        if (!workerType.IsAssignableTo<IBackgroundWorker>())
        {
            throw new Exception(
                $"Given type ({workerType.AssemblyQualifiedName}) must implement the {typeof(IBackgroundWorker).AssemblyQualifiedName} interface, but it doesn't!");
        }

        await context
            .GetRequiredService<IBackgroundWorkerManager>()
            .AddAsync((IBackgroundWorker)context.GetRequiredService(workerType), cancellationToken);
    }
}