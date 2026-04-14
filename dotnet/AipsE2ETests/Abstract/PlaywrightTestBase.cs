using AipsE2ETests.Infrastructure;
using Microsoft.Playwright.NUnit;

namespace AipsE2ETests.Abstract;

public abstract class PlaywrightTestBase : PageTest
{
    protected TestEnvironment TestEnvironment => GlobalSetup.TestEnvironment;
    
    protected const string BaseUrl = TestEnvironment.BaseUrl;
    protected const string WebApiUrl = TestEnvironment.WebApiUrl;

    [SetUp]
    public async Task BaseSetUp()
    {
        await TestEnvironment.ResetDatabaseAsync();
        
        await Page.GotoAsync(BaseUrl);
    }
}