namespace Domain.Notifications;

public enum NotificationSeverity : byte
{
    /// <summary>Info.</summary>
    Info,

    /// <summary>Success.</summary>
    Success,

    /// <summary>Warn.</summary>
    Warn,

    /// <summary>Error.</summary>
    Error,

    /// <summary>Fatal.</summary>
    Fatal,
}