namespace EventBus.Distributed;

public static class EqnDistributedEventBusExtensions
{
    public static ISupportsEventBoxes AsSupportsEventBoxes(this IDistributedEventBus eventBus)
    {
        var supportsEventBoxes = eventBus as ISupportsEventBoxes;
        if (supportsEventBoxes == null)
        {
            throw new Exception($"Given type ({eventBus.GetType().AssemblyQualifiedName}) should implement {nameof(ISupportsEventBoxes)}!");
        }

        return supportsEventBoxes;
    }
}
