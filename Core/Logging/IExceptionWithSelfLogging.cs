using Microsoft.Extensions.Logging;

namespace Core.Logging;

public interface IExceptionWithSelfLogging
{
    void Log(ILogger logger);
}
