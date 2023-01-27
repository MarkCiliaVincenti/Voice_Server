using Microsoft.Extensions.Logging;

namespace Core.Logging;

public interface IInitLogger<out T> : ILogger<T>
{
    public List<EqnInitLogEntry> Entries { get; }
}
