using AipsE2ETests.Abstract;
using Microsoft.Playwright;

namespace AipsE2ETests.Features.Auth;

[TestFixture]
public class LoginTests : PlaywrightTestBase
{
    private const string LoginPageUrl = $"{BaseUrl}/login";
    
    private const string UserName = "TestUser";
    private const string ValidEmail = "valid@email.com";
    private const string ValidPassword = "Password123!";
    
    [SetUp]
    public async Task SignupUser()
    {
        await CreateUser(UserName, ValidEmail, ValidPassword);
    }
    
    [Test]
    public async Task User_Can_Login_With_Valid_Credentials()
    {
        await Page.GotoAsync(LoginPageUrl, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        });
        
        var topbarLoginLink = Page.GetByTestId("topbar-login-link");
        var topbarSignupLink = Page.GetByTestId("topbar-signup-link");
        
        await Expect(topbarLoginLink).ToBeVisibleAsync();
        await Expect(topbarSignupLink).ToBeVisibleAsync();
        
        var form = Page.GetByTestId("login-form");
        
        await form.GetByTestId("login-email-input").FillAsync(ValidEmail);
        await form.GetByTestId("login-password-input").FillAsync(ValidPassword);
        
        await form.GetByTestId("login-submit-button").ClickAsync();
        
        await Page.WaitForURLAsync(BaseUrl);
        
        var topbarUsername = Page.GetByTestId("topbar-username");
        
        await Expect(topbarUsername).ToBeVisibleAsync();
        await Expect(topbarUsername).ToContainTextAsync(UserName);
        
        var topbarLogoutButton = Page.GetByTestId("topbar-logout-button");
        
        await Expect(topbarLogoutButton).ToBeVisibleAsync();
        
        await Expect(topbarLoginLink).Not.ToBeVisibleAsync();
        await Expect(topbarSignupLink).Not.ToBeVisibleAsync();
    }
}