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
                Arguments = "run --project ../AipsWebApi/AipsWebApi.csproj",
                WorkingDirectory = @"..\..\..\..\AipsWebApi",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        };
    }
}