namespace Domain.Notifications;

public class UserNotification
{
    public string UserId { get; set; }
    public NotificationSeverity Severity { get; set; }
    public Notification Notification { get; set; }
    public string[] Recipients { get; set; }
}

public class Notification
{
    public string Message { get; set; }
    public string Type { get; set; }
}