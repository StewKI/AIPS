using System.Diagnostics;

namespace AipsE2ETests.Infrastructure.Processes;

public sealed class AipsRTProcessService : IAsyncDisposable
{
    private Process? _process;

    public Task StartAsync(string db, string rabbit)
    {
        var env = EnvBuilder.CreateCommon(db, rabbit);

        _process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "run --project ../AipsRT/AipsRT.csproj",
                WorkingDirectory = @"..\..\..\..\AipsRT",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        };

        foreach (var kv in env)
            _process.StartInfo.Environment[kv.Key] = kv.Value;

        _process.Start();

        return Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        if (_process is { HasExited: false })
            _process.Kill(true);

        _process?.Dispose();
    }
}