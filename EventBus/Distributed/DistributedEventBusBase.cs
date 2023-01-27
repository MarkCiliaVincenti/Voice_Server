using Core;
using EventBus.Abstraction.EventBus;
using EventBus.Abstraction.EventBus.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Uow;

namespace EventBus.Distributed;

public abstract class DistributedEventBusBase : EventBusBase, IDistributedEventBus, ISupportsEventBoxes
{
    protected EqnDistributedEventBusOptions EqnDistributedEventBusOptions { get; }

    protected DistributedEventBusBase(
        IServiceScopeFactory serviceScopeFactory,
        IUnitOfWorkManager unitOfWorkManager,
        IOptions<EqnDistributedEventBusOptions> eqnDistributedEventBusOptions,
        IEventHandlerInvoker eventHandlerInvoker
    ) : base(
        serviceScopeFactory,
        unitOfWorkManager,
        eventHandlerInvoker)
    {
        EqnDistributedEventBusOptions = eqnDistributedEventBusOptions.Value;
    }

    public IDisposable Subscribe<TEvent>(IDistributedEventHandler<TEvent> handler) where TEvent : class
    {
        return Subscribe(typeof(TEvent), handler);
    }

    public override Task PublishAsync(Type eventType, object eventData, bool onUnitOfWorkComplete = true)
    {
        return PublishAsync(eventType, eventData, onUnitOfWorkComplete, useOutbox: true);
    }

    public Task PublishAsync<TEvent>(
        TEvent eventData,
        bool onUnitOfWorkComplete = true,
        bool useOutbox = true)
        where TEvent : class
    {
        return PublishAsync(typeof(TEvent), eventData, onUnitOfWorkComplete, useOutbox);
    }

    public async Task PublishAsync(
        Type eventType,
        object eventData,
        bool onUnitOfWorkComplete = true,
        bool useOutbox = true)
    {
        if (onUnitOfWorkComplete && UnitOfWorkManager.Current != null)
        {
            AddToUnitOfWork(
                UnitOfWorkManager.Current,
                new UnitOfWorkEventRecord(eventType, eventData, EventOrderGenerator.GetNext(), useOutbox)
            );
            return;
        }

        if (useOutbox)
        {
            if (await AddToOutboxAsync(eventType, eventData))
            {
                return;
            }
        }

        await PublishToEventBusAsync(eventType, eventData);
    }

    public abstract Task PublishFromOutboxAsync(
        OutgoingEventInfo outgoingEvent,
        OutboxConfig outboxConfig
    );

    public abstract Task PublishManyFromOutboxAsync(
        IEnumerable<OutgoingEventInfo> outgoingEvents,
        OutboxConfig outboxConfig
    );

    public abstract Task ProcessFromInboxAsync(
        IncomingEventInfo incomingEvent,
        InboxConfig inboxConfig);

    private async Task<bool> AddToOutboxAsync(Type eventType, object eventData)
    {
        var unitOfWork = UnitOfWorkManager.Current;
        if (unitOfWork == null)
        {
            return false;
        }

        foreach (var outboxConfig in EqnDistributedEventBusOptions.Outboxes.Values.OrderBy(x => x.Selector is null))
        {
            if (outboxConfig.Selector == null || outboxConfig.Selector(eventType))
            {
                var eventOutbox =
                    (IEventOutbox)unitOfWork.ServiceProvider.GetRequiredService(outboxConfig.ImplementationType);
                var eventName = EventNameAttribute.GetNameOrDefault(eventType);
                await eventOutbox.EnqueueAsync(
                    new OutgoingEventInfo(
                        Guid.NewGuid(),
                        eventName,
                        Serialize(eventData),
                        DateTime.Now
                    )
                );
                return true;
            }
        }

        return false;
    }

    protected async Task<bool> AddToInboxAsync(
        string messageId,
        string eventName,
        Type eventType,
        byte[] eventBytes)
    {
        if (EqnDistributedEventBusOptions.Inboxes.Count <= 0)
        {
            return false;
        }

        using (var scope = ServiceScopeFactory.CreateScope())
        {
            foreach (var inboxConfig in EqnDistributedEventBusOptions.Inboxes.Values.OrderBy(x =>
                         x.EventSelector is null))
            {
                if (inboxConfig.EventSelector == null || inboxConfig.EventSelector(eventType))
                {
                    var eventInbox =
                        (IEventInbox)scope.ServiceProvider.GetRequiredService(inboxConfig.ImplementationType);

                    if (!messageId.IsNullOrEmpty())
                    {
                        if (await eventInbox.ExistsByMessageIdAsync(messageId))
                        {
                            continue;
                        }
                    }

                    await eventInbox.EnqueueAsync(
                        new IncomingEventInfo(
                            Guid.NewGuid(),
                            messageId,
                            eventName,
                            eventBytes,
                            DateTime.Now
                        )
                    );
                }
            }
        }

        return true;
    }

    protected abstract byte[] Serialize(object eventData);
}