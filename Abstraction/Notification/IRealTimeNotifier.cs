using Domain.Notifications;

namespace Abstraction.Notification;

public interface IRealTimeNotifier
{
    Task SendNotificationAsync(UserNotification[] message);
}