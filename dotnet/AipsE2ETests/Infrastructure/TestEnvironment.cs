using AipsCore.Infrastructure.Persistence.Db;
using AipsE2ETests.Infrastructure.Processes;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace AipsE2ETests.Infrastructure;

public sealed partial class TestEnvironment : IAsyncDisposable
{
    private TestInfrastructure _infrastructure = null!;

    private readonly List<ProcessService> _processServices = [];
    
    protected HttpClient Client = null!;
    
    public const string BaseUrl = "http://localhost:5173";
    public const string WebApiUrl = "http://localhost:5266/api";

    internal TestEnvironment()
    {
        
    }
    
    internal void AddProcessService(ProcessService process)
    {
        _processServices.Add(process);
    }

    internal void SetInfrastructure(TestInfrastructure infrastructure)
    {
        _infrastructure = infrastructure;
    }

    public async Task InitializeAsync()
    {
        await MigrateAsync();

        foreach (var processService in _processServices)
        {
            await processService.StartProcessAsync();
        }
        
        Client = new HttpClient
        {
            BaseAddress = new Uri(WebApiUrl)
        };

        await WaitForWebApi();

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

    private async Task MigrateAsync()
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
    
    public async ValueTask DisposeAsync()
    {
        foreach (var processService in _processServices)
        {
            await processService.DisposeAsync();
        }
        
        Client.Dispose();
        
        await _infrastructure.DisposeAsync();
    }
}