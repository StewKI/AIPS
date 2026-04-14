using System.Diagnostics;

namespace AipsE2ETests.Infrastructure.Processes;

public sealed class AipsWebApiProcessService : IAsyncDisposable
{
    private Process? _process;

    public Task StartAsync(string db, string rabbit)
    {
        var env = EnvBuilder.CreateCommon(db, rabbit);
        
        env["ASPNETCORE_URLS"] = "http://localhost:5266";

        _process = new Process
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

        foreach (var kv in env)
        {
            _process.StartInfo.Environment[kv.Key] = kv.Value;
        }
        
        _process.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                TestContext.Progress.WriteLine($"[WEBAPI] {e.Data}");
            }
        };

        _process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                TestContext.Progress.WriteLine($"[WEBAPI ERROR] {e.Data}");
            }
        };
        
        _process.EnableRaisingEvents = true;

        _process.Exited += (_, _) =>
        {
            TestContext.Progress.WriteLine($"[WEBAPI EXITED] Code: {_process.ExitCode}");
        };

        _process.Start();
        
        _process.BeginOutputReadLine();
        _process.BeginErrorReadLine();

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