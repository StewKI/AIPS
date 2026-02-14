using System.Text;
using System.Text.Json;
using AipsCore.Application.Abstract;
using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Infrastructure.DI.Configuration;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace AipsCore.Infrastructure.MessageBroking.RabbitMQ;

public class RabbitMqPublisher : IMessagePublisher
{
    private readonly IRabbitMqConnection _connection;
    private readonly IConfiguration _configuration;

    public RabbitMqPublisher(IRabbitMqConnection connection, IConfiguration configuration)
    {
        _connection = connection;
        _configuration = configuration;
    }
    
    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
    {
        var channel = await _connection.CreateChannelAsync(cancellationToken);
        
        await channel.ExchangeDeclareAsync(GetExchange(), ExchangeType.Topic, durable: true, cancellationToken: cancellationToken);
        
        var bytes = Serialize(message);
        await channel.BasicPublishAsync(
            GetExchange(), 
            typeof(T).Name, 
            bytes, 
            cancellationToken);

        await channel.CloseAsync(cancellationToken);
    }

    private byte[] Serialize<T>(T message)
    {
        return JsonSerializer.SerializeToUtf8Bytes(message);
    }

    private string GetExchange() => _configuration.GetEnvRabbitMqExchange();
}