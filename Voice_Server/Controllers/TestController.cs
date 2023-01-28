using Abstraction.Notification;
using Abstraction.Storage;
using Core;
using EventBus.Distributed;
using Microsoft.AspNetCore.Mvc;
using Shared.Containers;
using Storage.BlobStoring;

namespace Voice_Server.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly IRealTimeNotifier _realTimeNotifier;
    private readonly IBlobContainer<ITestContainer> _blobContainer;
    private readonly IBlobContainer<ITest2Container> _testContainer;
    private readonly IDistributedEventBus _distributedEventBus;


    public TestController(IRealTimeNotifier realTimeNotifier,
        IBlobContainer<ITestContainer> blobContainer,
        IBlobContainer<ITest2Container> testContainer,
        IDistributedEventBus distributedEventBus)
    {
        _realTimeNotifier = realTimeNotifier;
        _blobContainer = blobContainer;
        _testContainer = testContainer;
        _distributedEventBus = distributedEventBus;
    }

    [HttpGet]
    public async Task<ActionResult<string>> Get()
    {
        //create a string and convert to byte 
        var test = "test";
        var test2 = "test2";
        var bytes = test.GetBytes();
        var bytes2 = test2.GetBytes();
        await _blobContainer.SaveAsync("busra" + ".txt", bytes, true);
        await _testContainer.SaveAsync("busra" + ".txt", bytes2, true);
        await _distributedEventBus.PublishAsync(new TestEvent
        {
            ProductId = 1
        });
        return "Hello World!";
    }
}