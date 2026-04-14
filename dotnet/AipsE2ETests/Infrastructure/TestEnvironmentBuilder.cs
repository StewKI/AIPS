using AipsE2ETests.Infrastructure.Processes;

namespace AipsE2ETests.Infrastructure;

public sealed class TestEnvironmentBuilder
{
    private readonly TestEnvironment _testEnvironment = new();
    private readonly TestInfrastructure _testInfrastructure = new();
    private readonly HashSet<Type> _addedServices = []; 
    private ProcessService? _lastService;

    private TestEnvironmentBuilder()
    {
        
    }
    
    public static async Task<TestEnvironmentBuilder> CreateAsync()
    {
        var builder = new TestEnvironmentBuilder();
        await builder._testInfrastructure.InitializeAsync();
        builder._testEnvironment.SetInfrastructure(builder._testInfrastructure);
        
        return builder;
    }
    
    private TestEnvironmentBuilder AddProcessService<TProcessService>(TProcessService processService) 
        where TProcessService : ProcessService
    {
        if (!_addedServices.Add(typeof(TProcessService)))
        {
            throw new InvalidOperationException($"Service of type {typeof(TProcessService).Name} has already been added to this TestEnvironment.");
        }

        _lastService = processService;
        _testEnvironment.AddProcessService(_lastService);
        return this;
    }
    
    public TestEnvironmentBuilder AddAipsWebApi() 
        => AddProcessService(new AipsWebApiProcessService(_testInfrastructure));

    public TestEnvironmentBuilder AddAipsRT() 
        => AddProcessService(new AipsRTProcessService(_testInfrastructure));

    public TestEnvironmentBuilder AddAipsWorker() 
        => AddProcessService(new AipsWorkerProcessService(_testInfrastructure));

    public TestEnvironmentBuilder AddFrontend() 
        => AddProcessService(new FrontendProcessService(_testInfrastructure));
    
    public TestEnvironmentBuilder WithOutputRedirectedToTestConsole()
    {
        _lastService?.RedirectOutputToTestConsole();
        return this;
    }

    public TestEnvironmentBuilder WithOutputRedirectedToTerminal()
    {
        _lastService?.RedirectOutputToTerminal();
        return this;
    }

    public TestEnvironment Build() => _testEnvironment;
}