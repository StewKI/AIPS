using AipsE2ETests.Infrastructure;
using Microsoft.Playwright.NUnit;

namespace AipsE2ETests.Abstract;

public abstract class PlaywrightTestBase : PageTest
{
    protected TestEnvironment TestEnvironment = null!;
    
    protected const string BaseUrl = TestEnvironment.BaseUrl;
    protected const string WebApiUrl = TestEnvironment.WebApiUrl;
    
    [OneTimeSetUp]
    public async Task GlobalSetup()
    {
        TestEnvironment = new TestEnvironment();
        await TestEnvironment.InitializeAsync();
    }
    
    [OneTimeTearDown]
    public async Task GlobalTeardown()
    {
        await TestEnvironment.DisposeAsync();
    }

    [SetUp]
    public async Task BaseSetUp()
    {
        await TestEnvironment.ResetDatabaseAsync();
    }
}