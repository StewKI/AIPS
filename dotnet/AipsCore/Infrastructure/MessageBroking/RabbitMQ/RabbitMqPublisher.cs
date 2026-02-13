using System.Text;
using System.Text.Json;
using AipsCore.Application.Abstract;
using AipsCore.Application.Abstract.MessageBroking;
using RabbitMQ.Client;

namespace AipsCore.Infrastructure.MessageBroking.RabbitMQ;

public class RabbitMqPublisher : IMessagePublisher
{
    private readonly IRabbitMqConnection _connection;

    public RabbitMqPublisher(IRabbitMqConnection connection)
    {
        _connection = connection;
    }
    
    public async Task PublishAsync<T>(string exchange, string routeKey, T message, CancellationToken cancellationToken = default)
    {
        var channel = await _connection.CreateChannelAsync(cancellationToken);
        
        await channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, cancellationToken: cancellationToken);
        
        var bytes = Serialize(message);
        await channel.BasicPublishAsync(exchange, routeKey, bytes, cancellationToken);

        await channel.CloseAsync(cancellationToken);
    }

    private byte[] Serialize<T>(T message)
    {
        return JsonSerializer.SerializeToUtf8Bytes(message);
    }
}