using AipsCore.Infrastructure.DI.Configuration;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace AipsCore.Infrastructure.MessageBroking.RabbitMQ;

public class RabbitMqConnection : IRabbitMqConnection
{
    private readonly IConfiguration _configuration;
    private IConnection? _connection;

    public RabbitMqConnection(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<IChannel> CreateChannelAsync(CancellationToken cancellationToken = default)
    {
        if (_connection is null)
        {
            await CreateConnectionAsync();
            //throw new InvalidOperationException($"RabbitMQ connection not created for {_configuration.GetEnvRabbitMqAmqpUri()}");
        }

        return await _connection!.CreateChannelAsync(null, cancellationToken);
    }

    public async Task CreateConnectionAsync()
    {
        var factory = CreateConnectionFactory();

        _connection = await factory.CreateConnectionAsync();
    }

    private IConnectionFactory CreateConnectionFactory()
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri(_configuration.GetEnvRabbitMqAmqpUri())
        };

        return factory;
    }
}