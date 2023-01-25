using Abstraction.Notification;
using Abstraction.Storage;
using Core;
using Domain.Notifications;
using Microsoft.AspNetCore.Mvc;
using Shared.Containers;
using Storage.BlobStoring;
using Volo.Abp.BlobStoring;

namespace Voice_Server.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly IRealTimeNotifier _realTimeNotifier;
    private readonly IBlobContainer<ITestContainer> _blobContainer;
   

    public TestController(IRealTimeNotifier realTimeNotifier, IBlobContainer<ITestContainer> blobContainer)
    {
        _realTimeNotifier = realTimeNotifier;
        _blobContainer = blobContainer;
     
    }

    [HttpGet]
    public async Task<ActionResult<string>> Get()
    {
        

        //create a string and convert to byte 
        var test = "test";
        var bytes = test.GetBytes();


        await _blobContainer.SaveAsync("test.txt", bytes);

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