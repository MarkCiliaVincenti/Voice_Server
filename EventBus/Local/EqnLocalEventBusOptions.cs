using Core.Collections;
using EventBus.Abstraction.EventBus;

namespace EventBus.Local;

public class EqnLocalEventBusOptions
{
    public ITypeList<IEventHandler> Handlers { get; }

    public EqnLocalEventBusOptions()
    {
        Handlers = new TypeList<IEventHandler>();
    }
}
