using System.Diagnostics;

namespace AipsE2ETests.Infrastructure.Processes;

public sealed class AipsWebApiProcessService : ProcessService
{
    public AipsWebApiProcessService(TestInfrastructure infrastructure) 
        : base(infrastructure)
    {
        
    }

    protected override string LogTag => "WEBAPI";

    protected override Process ConfigureProcess()
    {
        return new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "run --project ../AipsWebApi/AipsWebApi.csproj --launch-profile http",
                WorkingDirectory = @"..\..\..\..\AipsWebApi",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        };
    }
    
    protected override Dictionary<string, string> OverrideEnvironmentVariables()
    {
        return new Dictionary<string, string>
        {
            {"ASPNETCORE_URLS", "http://localhost:5266"},
            {"ASPNETCORE_ENVIRONMENT", "Production"},
            {"DOTNET_ENVIRONMENT", "Production"}
        };
    }
}