using System.Text;
using System.Text.Json;
using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Infrastructure.DI.Configuration;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AipsCore.Infrastructure.MessageBroking.RabbitMQ;

public class RabbitMqSubscriber : IMessageSubscriber
{
    private readonly IRabbitMqConnection _connection;
    private readonly IConfiguration _configuration;

    public RabbitMqSubscriber(IRabbitMqConnection connection, IConfiguration configuration)
    {
        _connection = connection;
        _configuration = configuration;
    }
    
    public async Task SubscribeAsync<T>(Func<T, CancellationToken, Task> handler)
    {
        var channel = await _connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(
            exchange: GetExchange(),
            type: ExchangeType.Topic,
            durable: true);

        await channel.QueueDeclareAsync(
            queue: GetQueueName<T>(),
            durable: true);
        
        await channel.QueueBindAsync(
            queue: GetQueueName<T>(),
            exchange: GetExchange(),
            routingKey: GetQueueName<T>());

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, args) =>
        {
            var message = Deserialize<T>(args.Body.ToArray());
            
            try
            {
                await handler(message, CancellationToken.None);

                await channel.BasicAckAsync(args.DeliveryTag, multiple: false);
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception);
                await channel.BasicNackAsync(args.DeliveryTag, multiple: false, requeue: false);
            }
        };

        await channel.BasicConsumeAsync(
            queue: GetQueueName<T>(),
            autoAck: false,
            consumer: consumer);
    }
    
    private string GetQueueName<T>()
    {
        return typeof(T).Name;
    }

    private string GetExchange()
    {
        return _configuration.GetEnvRabbitMqExchange();
    }

    private T Deserialize<T>(byte[] bytes)
    {
         var message = JsonSerializer.Deserialize<T>(bytes);

         if (message is null)
         {
             throw new Exception();
         }
         
         return message;
    }
}