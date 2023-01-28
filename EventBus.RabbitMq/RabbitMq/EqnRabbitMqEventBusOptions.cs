using RabbitMq;

namespace EventBus.RabbitMq.RabbitMq;

public class EqnRabbitMqEventBusOptions
{
    public const string DefaultExchangeType = RabbitMqConsts.ExchangeTypes.Direct;

    public string ConnectionName { get; set; }

    public string ClientName { get; set; }

    public string ExchangeName { get; set; }

    public string ExchangeType { get; set; }
    
    public ushort? PrefetchCount { get; set; }

    public string GetExchangeTypeOrDefault()
    {
        return string.IsNullOrEmpty(ExchangeType)
            ? DefaultExchangeType
            : ExchangeType;
    }
}
