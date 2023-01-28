using EventBus.Abstraction.EventBus;

namespace Voice_Server.Controllers;

[EventName("MyApp.Product.StockChange")]
public class TestEvent
{
    public int ProductId { get; set; }
}