﻿namespace EventBus.RabbitMq;

public interface IRabbitMqPersistentConnection
    : IDisposable
{
    bool IsConnected { get; }

    bool TryConnect();

    IModel CreateModel();
}
