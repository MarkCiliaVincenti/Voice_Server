namespace RabbitMq;

public class EqnRabbitMqOptions
{
    public RabbitMqConnections Connections { get; }

    public EqnRabbitMqOptions()
    {
        Connections = new RabbitMqConnections();
    }
}
