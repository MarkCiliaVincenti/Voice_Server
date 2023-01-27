using Core.Collections;
using EventBus.Abstraction.EventBus;

namespace EventBus.Distributed;

public class EqnDistributedEventBusOptions
{
    public ITypeList<IEventHandler> Handlers { get; }

    public OutboxConfigDictionary Outboxes { get; }

    public InboxConfigDictionary Inboxes { get; }
    public EqnDistributedEventBusOptions()
    {
        Handlers = new TypeList<IEventHandler>();
        Outboxes = new OutboxConfigDictionary();
        Inboxes = new InboxConfigDictionary();
    }
}
