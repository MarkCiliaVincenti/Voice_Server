using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMq;

public interface IRabbitMqMessageConsumer
{
    Task BindAsync(string routingKey);

    Task UnbindAsync(string routingKey);

    void OnMessageReceived(Func<IModel, BasicDeliverEventArgs, Task> callback);
}
