using System.Diagnostics;

namespace AipsE2ETests.Infrastructure.Processes;

public sealed class AipsWorkerProcessService : ProcessService
{
    public AipsWorkerProcessService(TestInfrastructure infrastructure) 
        : base(infrastructure)
    {
    }

    protected override string LogTag => "WORKER";
    
    protected override Process ConfigureProcess()
    {
        return new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "run --project ../AipsWorker/AipsWorker.csproj",
                WorkingDirectory = @"..\..\..\..\AipsWorker",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        };
    }
}