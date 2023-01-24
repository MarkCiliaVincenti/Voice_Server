using Domain.Notifications;

namespace Abstraction;

public interface IRealTimeNotifier
{
    Task SendNotificationAsync(UserNotification[] message);
}