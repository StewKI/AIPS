using DotNet.Testcontainers.Builders;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace AipsE2ETests.Infrastructure;

public sealed class TestInfrastructure 
{
    public PostgreSqlContainer Postgres { get; private set; } = null!;
    public RabbitMqContainer RabbitMq { get; private set; } = null!;
    
    public string PostgresConnectionString => Postgres.GetConnectionString();
    
    public string RabbitMqUri => $"amqp://guest:guest@localhost:{RabbitMq.GetMappedPublicPort(5672)}/";
    
    public async Task InitializeAsync()
    {
        Postgres = new PostgreSqlBuilder("postgres:latest")
            .WithDatabase("testDb")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        RabbitMq = new RabbitMqBuilder("rabbitmq:3-management")
            .WithUsername("guest")
            .WithPassword("guest")
            .WithPortBinding(5672, true)
            .WithPortBinding(15672, true)
            .Build();

        await Postgres.StartAsync();
        await RabbitMq.StartAsync();
    }
    
    public async Task DisposeAsync()
    {
        await RabbitMq.DisposeAsync();
        await Postgres.DisposeAsync();
    }
}