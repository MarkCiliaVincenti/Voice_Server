using Microsoft.Extensions.Logging;

namespace Application.Hubs;

public class OnlineClientHubBase : BaseHub
{
    private readonly ILogger<OnlineClientHubBase> _logger;

    public OnlineClientHubBase(ILogger<OnlineClientHubBase> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Client connected: {0}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Client disconnected: {0}", Context.ConnectionId);
        Console.WriteLine("Client disconnected: {0}", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}