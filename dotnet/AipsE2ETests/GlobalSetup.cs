using AipsE2ETests.Infrastructure;

namespace AipsE2ETests;

[SetUpFixture]
public class GlobalSetup
{
    public static TestEnvironment TestEnvironment { get; private set; }

    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        TestEnvironment = (await TestEnvironmentBuilder.CreateAsync())
            .AddAipsWebApi().WithOutputRedirectedToTestConsole()
            .AddAipsRT().WithOutputRedirectedToTestConsole()
            .AddAipsWorker().WithOutputRedirectedToTestConsole()
            .AddFrontend().WithOutputRedirectedToTestConsole()
            .Build();

        await TestEnvironment.InitializeAsync();
    }

    [OneTimeTearDown]
    public async Task RunAfterAllTests()
    {
        await TestEnvironment.DisposeAsync();
    }
}