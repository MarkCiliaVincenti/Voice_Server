using JetBrains.Annotations;

namespace Core.ExceptionHandling;

public interface IExceptionSubscriber
{
    Task HandleAsync([NotNull] ExceptionNotificationContext context);
}
