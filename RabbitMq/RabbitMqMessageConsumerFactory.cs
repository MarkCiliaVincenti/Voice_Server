using Microsoft.Extensions.DependencyInjection;

namespace RabbitMq;

public class RabbitMqMessageConsumerFactory : IRabbitMqMessageConsumerFactory
{
    protected IServiceProvider Provider { get; }

    public RabbitMqMessageConsumerFactory(IServiceProvider provider)
    {
        Provider = provider;
    }

    public IRabbitMqMessageConsumer Create(
        ExchangeDeclareConfiguration exchange,
        QueueDeclareConfiguration queue,
        string connectionName = null)
    {
        var consumer = Provider.GetRequiredService<RabbitMqMessageConsumer>();
        consumer.Initialize(exchange, queue, connectionName);
        return consumer;
    }
}