using AipsE2ETests.Abstract;
using Microsoft.Playwright;

namespace AipsE2ETests.Features.Auth;

[TestFixture]
public class LogInTests : PlaywrightTestBase
{
    private const string LoginPageUrl = $"{BaseUrl}/login";
    
    private const string UserName = "TestUser";
    private const string ValidEmail = "valid@email.com";
    private const string ValidPassword = "Password123!";
    
    private const string InvalidEmail = "invalid@email";
    private const string InvalidPassword = "password";
    
    private const string InvalidCredentialsError = "Invalid credentials";
    
    private ILocator _topbarLoginLink = null!;
    private ILocator _topbarSignupLink = null!;
    
    private ILocator _topbarUsername = null!;
    private ILocator _topbarLogoutButton = null!;
        
    private ILocator _loginError = null!;
    
    private ILocator _form = null!;
    private ILocator _emailInput = null!;
    private ILocator _passwordInput = null!;
    private ILocator _submitButton = null!;
    
    [SetUp]
    public async Task LogInTestsSetUp()
    {
        await TestEnvironment.CreateUser(UserName, ValidEmail, ValidPassword);
        
        await Page.GotoAsync(LoginPageUrl, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        });
        
        _topbarLoginLink = Page.GetByTestId("topbar-login-link");
        _topbarSignupLink = Page.GetByTestId("topbar-signup-link");
        
        _topbarUsername = Page.GetByTestId("topbar-username");
        _topbarLogoutButton = Page.GetByTestId("topbar-logout-button");
        
        _loginError = Page.GetByTestId("login-error");
        
        _form = Page.GetByTestId("login-form");
        _emailInput = _form.GetByTestId("login-email-input");
        _passwordInput = _form.GetByTestId("login-password-input");
        _submitButton = _form.GetByTestId("login-submit-button");
    }
    
    [Test]
    public async Task User_Can_Login_With_Valid_Credentials()
    {
        await Expect(_topbarLoginLink).ToBeVisibleAsync();
        await Expect(_topbarSignupLink).ToBeVisibleAsync();
        await Expect(_topbarUsername).Not.ToBeVisibleAsync();
        await Expect(_topbarLogoutButton).Not.ToBeVisibleAsync();
        
        await _emailInput.FillAsync(ValidEmail);
        await _passwordInput.FillAsync(ValidPassword);
        
        await _submitButton.ClickAsync();
        
        await Page.WaitForURLAsync(BaseUrl);
        
        await Expect(_topbarLogoutButton).ToBeVisibleAsync();
        await Expect(_topbarUsername).ToBeVisibleAsync();
        await Expect(_topbarUsername).ToContainTextAsync(UserName);
        
        await Expect(_topbarLoginLink).Not.ToBeVisibleAsync();
        await Expect(_topbarSignupLink).Not.ToBeVisibleAsync();
        
        await Expect(_loginError).Not.ToBeVisibleAsync();
    }

    [Test]
    public async Task User_Cannot_Login_With_Invalid_Email()
    {
        await Expect(_topbarLoginLink).ToBeVisibleAsync();
        await Expect(_topbarSignupLink).ToBeVisibleAsync();
        await Expect(_topbarUsername).Not.ToBeVisibleAsync();
        await Expect(_topbarLogoutButton).Not.ToBeVisibleAsync();
        
        await _emailInput.FillAsync(InvalidEmail);
        await _passwordInput.FillAsync(ValidPassword);
        
        await _submitButton.ClickAsync();
        
        await Expect(_topbarLoginLink).ToBeVisibleAsync();
        await Expect(_topbarSignupLink).ToBeVisibleAsync();
        await Expect(_topbarUsername).Not.ToBeVisibleAsync();
        await Expect(_topbarLogoutButton).Not.ToBeVisibleAsync();
        
        await Expect(_loginError).ToBeVisibleAsync();
        await Expect(_loginError.Locator("li")).ToHaveCountAsync(1);
        await Expect(_loginError).ToContainTextAsync(InvalidCredentialsError);
    }
    
    [Test]
    public async Task User_Cannot_Login_With_Invalid_Password()
    {
        await Expect(_topbarLoginLink).ToBeVisibleAsync();
        await Expect(_topbarSignupLink).ToBeVisibleAsync();
        await Expect(_topbarUsername).Not.ToBeVisibleAsync();
        await Expect(_topbarLogoutButton).Not.ToBeVisibleAsync();
        
        await _emailInput.FillAsync(ValidEmail);
        await _passwordInput.FillAsync(InvalidPassword);
        
        await _submitButton.ClickAsync();
        
        await Expect(_topbarLoginLink).ToBeVisibleAsync();
        await Expect(_topbarSignupLink).ToBeVisibleAsync();
        await Expect(_topbarUsername).Not.ToBeVisibleAsync();
        await Expect(_topbarLogoutButton).Not.ToBeVisibleAsync();
        
        await Expect(_loginError).ToBeVisibleAsync();
        await Expect(_loginError.Locator("li")).ToHaveCountAsync(1);
        await Expect(_loginError).ToContainTextAsync(InvalidCredentialsError);
    }
}