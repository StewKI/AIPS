using System.Diagnostics.CodeAnalysis;
using AipsCore.Infrastructure.DI;
using AipsCore.Infrastructure.Persistence.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace AipsIntegrationTests.Abstract;

[TestFixture]
[SuppressMessage("NUnit.Framework", "NUnit1032")]
public abstract class IntegrationTestBase
{
    private static readonly PostgreSqlContainer PgContainer = new PostgreSqlBuilder("postgres:latest")
        .WithDatabase("aips_db_test")
        .WithUsername("postgres_test")
        .WithPassword("postgres_test")
        .Build();

    private IServiceProvider? ServiceProvider { get; set; }
    private IServiceScope? Scope { get; set; }

    [OneTimeSetUp]
    public async Task GlobalSetup()
    {
        await PgContainer.StartAsync();
    }
    
    [OneTimeTearDown]
    public async Task GlobalTeardown()
    {
        await PgContainer.StopAsync();
    }

    [SetUp]
    public async Task BaseSetUp()
    {
        var services = new ServiceCollection();
        
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["DB_CONN_STRING"] = PgContainer.GetConnectionString(),
                ["JWT_ISSUER"] = "TestIssuer",
                ["JWT_AUDIENCE"] = "TestAudience",
                ["JWT_KEY"] = "super_secret_test_key_that_is_long_enough",
                ["JWT_EXPIRATION_MINUTES"] = "60",
                ["JWT_REFRESH_EXPIRATION_DAYS"] = "7",
                ["RABBITMQ_AMQP_URI"] = "amqp://guest:guest@localhost:5672",
                ["RABBITMQ_EXCHANGE"] = "rabbitmq_exchange_test",
                ["RABBITMQ_DEFAULT_USER"] = "rabbitmq_user_test",
                ["RABBITMQ_DEFAULT_PASSWORD"] = "rabbitmq_password_test",
                ["RABBITMQ_DEFAULT_VHOST"] = "/",
            })
            .Build();
        
        services.AddSingleton<IConfiguration>(configuration);
        
        services.AddAips(configuration);
        
        InterceptServices(services);

        ServiceProvider = services.BuildServiceProvider();
        Scope = ServiceProvider.CreateScope();
        
        var dbContext = GetService<AipsDbContext>();

        await dbContext.Database.MigrateAsync();
        
        await ResetDatabaseAsync(dbContext);
        
        await DbInitializer.SeedRolesAsync(ServiceProvider);
    }
    
    [TearDown]
    public void BaseTearDown()
    {
        Scope?.Dispose();
        (ServiceProvider as IDisposable)?.Dispose();
        
        Scope = null;
        ServiceProvider = null;
    }

    protected TService GetService<TService>()
        where TService : notnull
    {
        if (Scope == null) throw new InvalidOperationException("Scope is not initialized");
        
        return Scope.ServiceProvider.GetRequiredService<TService>();
    }

    protected virtual void InterceptServices(IServiceCollection services)
    {
        
    }
    
    private async Task ResetDatabaseAsync(AipsDbContext dbContext)
    {
        await dbContext.Database.ExecuteSqlRawAsync(@"
            DO $$
            DECLARE
                stmt TEXT;
            BEGIN
                SELECT INTO stmt
                    'TRUNCATE TABLE ' || string_agg(format('%I.%I', schemaname, tablename), ', ') || ' RESTART IDENTITY CASCADE;'
                FROM pg_tables
                WHERE schemaname IN ('public')
                AND tablename != '__EFMigrationsHistory';

                IF stmt IS NOT NULL THEN
                    EXECUTE stmt;
                END IF;
            END $$;
            ");
    }
}