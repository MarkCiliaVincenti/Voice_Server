namespace EventBus.Abstraction.Domain.Entities.Events.Distributed;

[Serializable]
public abstract class EtoBase
{
    public Dictionary<string, string> Properties { get; set; }

    protected EtoBase()
    {
        Properties = new Dictionary<string, string>();
    }
}
