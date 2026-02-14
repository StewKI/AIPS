using RabbitMQ.Client;

namespace AipsCore.Infrastructure.MessageBroking.RabbitMQ;

public interface IRabbitMqConnection
{
    Task<IChannel> CreateChannelAsync(CancellationToken cancellationToken = default);
}