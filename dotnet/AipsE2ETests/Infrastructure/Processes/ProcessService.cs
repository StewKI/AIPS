using System.Diagnostics;

namespace AipsE2ETests.Infrastructure.Processes;

public abstract class ProcessService : IProcessService
{
    private enum RedirectionType
    {
        None,
        ToTerminal,
        ToTestConsole
    }
    
    private Process? _process;
    private RedirectionType _redirectionType = RedirectionType.None;
    
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

        RedirectOutput();
        
        _process.Start();
        
        if (_process.StartInfo.RedirectStandardOutput)
        {
            _process.BeginOutputReadLine();
        }
        
        if (_process.StartInfo.RedirectStandardError)
        {
            _process.BeginErrorReadLine();
        }

        return Task.CompletedTask;
    }

    private void RedirectOutput()
    {
        switch (_redirectionType)
        {
            case RedirectionType.None:
                break;
            case RedirectionType.ToTerminal:
                ApplyRedirectOutputToTerminal();
                break;
            case RedirectionType.ToTestConsole:
                ApplyRedirectOutputToTestConsole();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
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

    public void RedirectOutputToTerminal()
    {
        _redirectionType = RedirectionType.ToTerminal;
    }

    public void RedirectOutputToTestConsole()
    {
        _redirectionType = RedirectionType.ToTestConsole;
    }
    
    private void ApplyRedirectOutputToTerminal()
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

    private void ApplyRedirectOutputToTestConsole()
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