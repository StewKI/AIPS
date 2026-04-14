using System.Diagnostics;

namespace AipsE2ETests.Infrastructure.Processes;

public sealed class FrontendProcessService : ProcessService
{
    public FrontendProcessService(TestInfrastructure infrastructure) 
        : base(infrastructure)
    {
    }

    protected override string LogTag => "FRONT";
    
    protected override Process ConfigureProcess()
    {
        return new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "bun",
                Arguments = "run dev",
                WorkingDirectory = @"..\..\..\..\..\front",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        };
    }
}