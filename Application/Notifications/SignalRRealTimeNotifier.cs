using Abstraction.Notification;
using Application.Hubs;
using Domain;
using Domain.Notifications;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Application.Notifications;

public class SignalRRealTimeNotifier : IRealTimeNotifier
{
    private readonly IHubContext<OnlineClientHubBase> _hubContext;
    private readonly ILogger<SignalRRealTimeNotifier> _logger;

    public SignalRRealTimeNotifier(IHubContext<OnlineClientHubBase> hubContext,
        ILogger<SignalRRealTimeNotifier> logger
    )
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task SendNotificationAsync(UserNotification[] messages)
    {
        foreach (var message in messages)
        {
            try
            {
                ValidateMessage(message);
                var onlineClient = UserContext.ActiveUser;
                await _hubContext.Clients.Client(message.UserId)
                    .SendAsync("ReceiveNotification", message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error sending notification to user {UserId}", message.UserId);
            }
        }
    }

    private void ValidateMessage(UserNotification message)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(message.UserId);
        ArgumentNullException.ThrowIfNullOrEmpty(message.Notification.Message);
    }
}