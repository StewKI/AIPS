using System.Net.Http.Json;
using AipsCore.Infrastructure.Persistence.Db;
using AipsE2ETests.Infrastructure;
using AipsE2ETests.Infrastructure.Processes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Playwright.NUnit;
using Npgsql;

namespace AipsE2ETests.Abstract;

public abstract class PlaywrightTestBase : PageTest
{
    protected const string BaseUrl = "http://localhost:5173";
    protected const string WebApiUrl = "http://localhost:5266/api";

    private TestInfrastructure _infrastructure = null!;
    
    protected AipsWebApiProcessService Api = null!;
    protected AipsRTProcessService Rt = null!;
    protected AipsWorkerProcessService Worker = null!;
    protected FrontendProcessService Frontend = null!;
    
    protected HttpClient Client = null!;
    
    [OneTimeSetUp]
    public async Task GlobalSetup()
    {
        _infrastructure = new TestInfrastructure();
        await _infrastructure.InitializeAsync();
        
        await MigrateAsync();

        Api = new AipsWebApiProcessService();
        Rt = new AipsRTProcessService();
        Worker = new AipsWorkerProcessService();
        Frontend = new FrontendProcessService();
        
        await Api.StartAsync(_infrastructure.PostgresConnectionString, _infrastructure.RabbitMqUri);
        await Rt.StartAsync(_infrastructure.PostgresConnectionString, _infrastructure.RabbitMqUri);
        await Worker.StartAsync(_infrastructure.PostgresConnectionString, _infrastructure.RabbitMqUri);

        
        Client = new HttpClient
        {
            BaseAddress = new Uri(WebApiUrl)
        };

        await WaitForWebApi();
        
        await Frontend.StartAsync();

        await Task.Delay(5000);
    }
    
    [OneTimeTearDown]
    public async Task GlobalTeardown()
    {
        await Frontend.DisposeAsync();
        await Worker.DisposeAsync();
        await Rt.DisposeAsync();
        await Api.DisposeAsync();

        Client.Dispose();
        await _infrastructure.DisposeAsync();
    }

    [SetUp]
    public async Task BaseSetUp()
    {
        await ResetDatabaseAsync();
    }
    
    private async Task WaitForWebApi()
    {
        await WaitHelper.UntilAsync(async () =>
        {
            try
            {
                var res = await Client.GetAsync("/health");
                return res.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }, TimeSpan.FromSeconds(60));
    }
    
    protected async Task CreateUser(string username, string email, string password)
    {
        var response = await Client.PostAsJsonAsync("/api/auth/signup", new
        {
            username,
            email,
            password
        });

        response.EnsureSuccessStatusCode();
    }
    
    protected async Task MigrateAsync()
    {
        var connString = _infrastructure.PostgresConnectionString;

        var options = new DbContextOptionsBuilder<AipsDbContext>()
            .UseNpgsql(connString)
            .Options;

        await using var db = new AipsDbContext(options);

        await db.Database.MigrateAsync();
    }
    
    protected async Task ResetDatabaseAsync()
    {
        var connString = _infrastructure.PostgresConnectionString;

        await using var conn = new NpgsqlConnection(connString);
        await conn.OpenAsync();

        var sql = @"
            DO $$
            DECLARE
                stmt TEXT;
            BEGIN
                SELECT INTO stmt
                    'TRUNCATE TABLE ' || string_agg(format('%I.%I', schemaname, tablename), ', ') || ' RESTART IDENTITY CASCADE;'
                FROM pg_tables
                WHERE schemaname IN ('public')
                AND tablename != '__EFMigrationsHistory'
                AND tablename != 'AspNetRoles';   

                IF stmt IS NOT NULL THEN
                    EXECUTE stmt;
                END IF;
            END $$;
            ";

        await using var cmd = new NpgsqlCommand(sql, conn);
        await cmd.ExecuteNonQueryAsync();
    }
}