namespace Core.ExceptionHandling;

public abstract class ExceptionSubscriber : IExceptionSubscriber
{
    public abstract Task HandleAsync(ExceptionNotificationContext context);
}
