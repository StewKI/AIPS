using System.Diagnostics;

namespace AipsE2ETests.Infrastructure.Processes;

public sealed class AipsRTProcessService : ProcessService
{
    public AipsRTProcessService(TestInfrastructure infrastructure) 
        : base(infrastructure)
    {
    }

    protected override string LogTag => "RT";
    
    protected override Process ConfigureProcess()
    {
        return new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "run --project ../AipsRT/AipsRT.csproj --launch-profile http",
                WorkingDirectory = @"..\..\..\..\AipsRT",
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
            {"ASPNETCORE_URLS", "http://localhost:5039"},
            {"ASPNETCORE_ENVIRONMENT", "Production"},
            {"DOTNET_ENVIRONMENT", "Production"}
        };
    }
}