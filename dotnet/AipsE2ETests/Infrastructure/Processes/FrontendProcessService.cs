using System.Diagnostics;

namespace AipsE2ETests.Infrastructure.Processes;

public sealed class FrontendProcessService : IAsyncDisposable
{
    private Process? _process;

    public Task StartAsync()
    {
        _process = new Process
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

        _process.Start();

        return Task.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        try
        {
            if (_process is { HasExited: false })
            {
                _process.Kill(true);
            }

            _process?.Dispose();
            
            return ValueTask.CompletedTask;
        }
        catch (Exception exception)
        {
            return ValueTask.FromException(exception);
        }
    }
}