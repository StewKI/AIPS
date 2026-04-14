using System.Net.Http.Json;
using AipsCore.Infrastructure.Persistence.Db;
using AipsE2ETests.Infrastructure.Processes;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace AipsE2ETests.Infrastructure;

public class TestEnvironment : IAsyncDisposable
{
    private TestInfrastructure _infrastructure = null!;
    
    protected AipsWebApiProcessService WebApi = null!;
    protected AipsRTProcessService Rt = null!;
    protected AipsWorkerProcessService Worker = null!;
    protected FrontendProcessService Frontend = null!;
    
    protected HttpClient Client = null!;
    
    public const string BaseUrl = "http://localhost:5173";
    public const string WebApiUrl = "http://localhost:5266/api";

    public async Task InitializeAsync()
    {
        _infrastructure = new TestInfrastructure();
        await _infrastructure.InitializeAsync();
        
        await MigrateAsync();

        WebApi = new AipsWebApiProcessService(_infrastructure);
        Rt = new AipsRTProcessService(_infrastructure);
        Worker = new AipsWorkerProcessService(_infrastructure);
        Frontend = new FrontendProcessService(_infrastructure);

        await WebApi.StartProcessAsync();
        await Rt.StartProcessAsync();
        await Worker.StartProcessAsync();
        
        WebApi.RedirectOutputToTestConsole();
        Rt.RedirectOutputToTestConsole();
        Worker.RedirectOutputToTestConsole();
        
        Client = new HttpClient
        {
            BaseAddress = new Uri(WebApiUrl)
        };

        await WaitForWebApi();
        
        await Frontend.StartProcessAsync();

        await Task.Delay(5000);
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
    
    protected async Task MigrateAsync()
    {
        var connString = _infrastructure.PostgresConnectionString;

        var options = new DbContextOptionsBuilder<AipsDbContext>().UseNpgsql(connString).Options;

        await using var db = new AipsDbContext(options);

        await db.Database.MigrateAsync();
    }
    
    public async Task ResetDatabaseAsync()
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
    
    public async Task CreateUser(string username, string email, string password)
    {
        var response = await Client.PostAsJsonAsync("/api/auth/signup", new
        {
            username,
            email,
            password
        });

        response.EnsureSuccessStatusCode();
    }
    
    public async ValueTask DisposeAsync()
    {
        await Worker.DisposeAsync();
        await Rt.DisposeAsync();
        await WebApi.DisposeAsync();
        
        Client.Dispose();
        
        await Frontend.DisposeAsync();
        await _infrastructure.DisposeAsync();
    }
}