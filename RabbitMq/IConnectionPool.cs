using RabbitMQ.Client;

namespace RabbitMq;

public interface IConnectionPool : IDisposable
{
    IConnection Get(string connectionName = null);
}
