using Abstraction;
using Domain.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace Voice_Server.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly IRealTimeNotifier _realTimeNotifier;

    public TestController(IRealTimeNotifier realTimeNotifier)
    {
        _realTimeNotifier = realTimeNotifier;
    }

    [HttpGet]
    public ActionResult<string> Get()
    {
        _realTimeNotifier.SendNotificationAsync(new[]
        {
            new UserNotification
            {
                UserId = "1",
                Notification = new Notification
                {
                    Message = "Hello World",
                }
            }
        });
        return "Hello World!";
    }
}