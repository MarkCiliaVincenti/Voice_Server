using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus.RabbitMq.RabbitMq;

public static class EqnEventBusRabbitMqModule
{
    public static void ConfigureServices(this WebApplicationBuilder context)
    {
        //Configure<EqnRabbitMqEventBusOptions>(configuration.GetSection("RabbitMQ:EventBus"));
    }

    public static void OnApplicationInitialization(IServiceProvider context)
    {
        context
            .GetRequiredService<RabbitMqDistributedEventBus>()
            .Initialize();
    }
}