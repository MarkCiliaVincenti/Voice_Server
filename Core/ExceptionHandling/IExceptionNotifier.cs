using JetBrains.Annotations;

namespace Core.ExceptionHandling;

public interface IExceptionNotifier
{
    Task NotifyAsync([NotNull] ExceptionNotificationContext context);
}
