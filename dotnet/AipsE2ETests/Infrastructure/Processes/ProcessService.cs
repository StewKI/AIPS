using System.Diagnostics;

namespace AipsE2ETests.Infrastructure.Processes;

public abstract class ProcessService : IProcessService
{
    private Process? _process;
    private readonly TestInfrastructure _infrastructure;
    
    protected abstract string LogTag { get; }
    
    protected ProcessService(TestInfrastructure infrastructure)
    {
        _infrastructure = infrastructure;
    }

    public Task StartProcessAsync()
    {
        _process = ConfigureProcess();
        
        SetEnvironmentVariables();
        
        _process.Start();
        
        _process.BeginOutputReadLine();
        _process.BeginErrorReadLine();

        return Task.CompletedTask;
    }

    protected abstract Process ConfigureProcess();

    protected virtual Dictionary<string, string> OverrideEnvironmentVariables()
    {
        return new Dictionary<string, string>();
    }

    private void SetEnvironmentVariables()
    {
        if (_process is null)
        {
            throw new InvalidOperationException("Process is not initialized.");
        }
        
        var defaultEnv = EnvBuilder.CreateCommon(_infrastructure.PostgresConnectionString, _infrastructure.RabbitMqUri);

        var overrideEnv = OverrideEnvironmentVariables();
        
        foreach (var kv in defaultEnv)
        {
            _process.StartInfo.Environment[kv.Key] = kv.Value;
        }
        
        foreach (var kv in overrideEnv)
        {
            _process.StartInfo.Environment[kv.Key] = kv.Value;
        }
    }

    public void RedirectOutputToConsole()
    {
        if (_process is null)
        {
            throw new InvalidOperationException("Process is not initialized.");
        }
        
        _process.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                Console.WriteLine($"[{LogTag}] {e.Data}");
            }
        };

        _process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                Console.WriteLine($"[{LogTag} ERROR] {e.Data}");
            }
        };
        
        _process.EnableRaisingEvents = true;

        _process.Exited += (_, _) =>
        {
            Console.WriteLine($"[{LogTag} EXITED] Code: {_process.ExitCode}");
        };
    }

    public void RedirectOutputToTestConsole()
    {
        if (_process is null)
        {
            throw new InvalidOperationException("Process is not initialized.");
        }
        
        _process.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                TestContext.Progress.WriteLine($"[{LogTag}] {e.Data}");
            }
        };

        _process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                TestContext.Progress.WriteLine($"[{LogTag} ERROR] {e.Data}");
            }
        };
        
        _process.EnableRaisingEvents = true;

        _process.Exited += (_, _) =>
        {
            TestContext.Progress.WriteLine($"[{LogTag} EXITED] Code: {_process.ExitCode}");
        };
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