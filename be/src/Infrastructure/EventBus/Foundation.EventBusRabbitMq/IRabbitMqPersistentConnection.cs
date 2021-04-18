using RabbitMQ.Client;
using System;

namespace Foundation.EventBusRabbitMq
{
    public interface IRabbitMqPersistentConnection
        : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
