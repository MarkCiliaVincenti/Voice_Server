namespace EventBus.Distributed;

public interface IInboxProcessor
{
    Task StartAsync(InboxConfig inboxConfig, CancellationToken cancellationToken = default);

    Task StopAsync(CancellationToken cancellationToken = default);
}
