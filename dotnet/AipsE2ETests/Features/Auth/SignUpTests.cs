using AipsE2ETests.Abstract;
using Microsoft.Playwright;

namespace AipsE2ETests.Features.Auth;

[TestFixture]
public class SignUpTests : PlaywrightTestBase
{
    private const string SignUpPageUrl = $"{BaseUrl}/signup";
    
    private const string ValidUserName = "TestUser";
    private const string ValidEmail = "valid@email.com";
    private const string ValidPassword = "Password123!";
    
    private const string InvalidUserName = "U S E R";
    private const string InvalidPassword = "password";

    private const string ValidNonExistingUserName = "TestUser2";
    private const string ValidNonExistingEmail = "valid@email2.com";
    
    private const string ExistingUserName = ValidUserName;
    private const string ExistingEmail = ValidEmail;

    private const string ExistingUserNameError = $"Username '{ExistingUserName}' is already taken.";
    private const string ExistingEmailError = $"Email '{ExistingEmail}' is already taken.";
    
    private ILocator _topbarLoginLink = null!;
    private ILocator _topbarSignupLink = null!;
    
    private ILocator _topbarUsername = null!;
    private ILocator _topbarLogoutButton = null!;
    
    private ILocator _signupError = null!;
    
    private ILocator _form = null!;
    private ILocator _usernameInput = null!;
    private ILocator _emailInput = null!;
    private ILocator _passwordInput = null!;
    private ILocator _submitButton = null!;
    
    [SetUp]
    public async Task SignUpTestsSetUp()
    {
        await Page.GotoAsync(SignUpPageUrl, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        });
        
        _topbarLoginLink = Page.GetByTestId("topbar-login-link");
        _topbarSignupLink = Page.GetByTestId("topbar-signup-link");
        
        _topbarUsername = Page.GetByTestId("topbar-username");
        _topbarLogoutButton = Page.GetByTestId("topbar-logout-button");
        
        _signupError = Page.GetByTestId("signup-error");
        
        _form = Page.GetByTestId("signup-form");
        _usernameInput = _form.GetByTestId("signup-username-input");
        _emailInput = _form.GetByTestId("signup-email-input");
        _passwordInput = _form.GetByTestId("signup-password-input");
        _submitButton = _form.GetByTestId("signup-submit-button");
    }

    [Test]
    public async Task User_Can_Sign_Up_With_Valid_Credentials()
    {
        await Expect(_topbarLoginLink).ToBeVisibleAsync();
        await Expect(_topbarSignupLink).ToBeVisibleAsync();
        await Expect(_topbarUsername).Not.ToBeVisibleAsync();
        await Expect(_topbarLogoutButton).Not.ToBeVisibleAsync();
        
        await _usernameInput.FillAsync(ValidUserName);
        await _emailInput.FillAsync(ValidEmail);
        await _passwordInput.FillAsync(ValidPassword);
        
        await _submitButton.ClickAsync();
        
        await Page.WaitForURLAsync(BaseUrl);
        
        await Expect(_topbarLogoutButton).ToBeVisibleAsync();
        await Expect(_topbarUsername).ToBeVisibleAsync();
        await Expect(_topbarUsername).ToContainTextAsync(ValidUserName);
        
        await Expect(_topbarLoginLink).Not.ToBeVisibleAsync();
        await Expect(_topbarSignupLink).Not.ToBeVisibleAsync();
        
        await Expect(_signupError).Not.ToBeVisibleAsync();
    }
    
    [Test]
    public async Task User_Cannot_Sign_Up_With_Invalid_UserName()
    {
        await Expect(_topbarLoginLink).ToBeVisibleAsync();
        await Expect(_topbarSignupLink).ToBeVisibleAsync();
        await Expect(_topbarUsername).Not.ToBeVisibleAsync();
        await Expect(_topbarLogoutButton).Not.ToBeVisibleAsync();
        
        await _usernameInput.FillAsync(InvalidUserName);
        await _emailInput.FillAsync(ValidEmail);
        await _passwordInput.FillAsync(ValidPassword);
        
        await _submitButton.ClickAsync();
        
        await Expect(_topbarLoginLink).ToBeVisibleAsync();
        await Expect(_topbarSignupLink).ToBeVisibleAsync();
        await Expect(_topbarUsername).Not.ToBeVisibleAsync();
        await Expect(_topbarLogoutButton).Not.ToBeVisibleAsync();
        
        await Expect(_signupError).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task User_Cannot_Sign_Up_With_Invalid_Password()
    {
        await Expect(_topbarLoginLink).ToBeVisibleAsync();
        await Expect(_topbarSignupLink).ToBeVisibleAsync();
        await Expect(_topbarUsername).Not.ToBeVisibleAsync();
        await Expect(_topbarLogoutButton).Not.ToBeVisibleAsync();
        
        await _usernameInput.FillAsync(ValidUserName);
        await _emailInput.FillAsync(ValidEmail);
        await _passwordInput.FillAsync(InvalidPassword);
        
        await _submitButton.ClickAsync();
        
        await Expect(_topbarLoginLink).ToBeVisibleAsync();
        await Expect(_topbarSignupLink).ToBeVisibleAsync();
        await Expect(_topbarUsername).Not.ToBeVisibleAsync();
        await Expect(_topbarLogoutButton).Not.ToBeVisibleAsync();
        
        await Expect(_signupError).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task User_Cannot_Sign_Up_With_Existing_Email()
    {
        await TestEnvironment.CreateUser(ValidUserName, ValidEmail, ValidPassword);       
        
        await Expect(_topbarLoginLink).ToBeVisibleAsync();
        await Expect(_topbarSignupLink).ToBeVisibleAsync();
        await Expect(_topbarUsername).Not.ToBeVisibleAsync();
        await Expect(_topbarLogoutButton).Not.ToBeVisibleAsync();
        
        await _usernameInput.FillAsync(ValidNonExistingUserName);
        await _emailInput.FillAsync(ExistingEmail);
        await _passwordInput.FillAsync(ValidPassword);
        
        await _submitButton.ClickAsync();
        
        await Expect(_topbarLoginLink).ToBeVisibleAsync();
        await Expect(_topbarSignupLink).ToBeVisibleAsync();
        await Expect(_topbarUsername).Not.ToBeVisibleAsync();
        await Expect(_topbarLogoutButton).Not.ToBeVisibleAsync();
        
        await Expect(_signupError).ToBeVisibleAsync();
        await Expect(_signupError.Locator("li")).ToHaveCountAsync(1);
        await Expect(_signupError).ToContainTextAsync(ExistingEmailError);
    }
    
    [Test]
    public async Task User_Cannot_Sign_Up_With_Existing_UserName()
    {
        await TestEnvironment.CreateUser(ValidUserName, ValidEmail, ValidPassword);
        
        await Expect(_topbarLoginLink).ToBeVisibleAsync();
        await Expect(_topbarSignupLink).ToBeVisibleAsync();
        await Expect(_topbarUsername).Not.ToBeVisibleAsync();
        await Expect(_topbarLogoutButton).Not.ToBeVisibleAsync();
        
        await _usernameInput.FillAsync(ExistingUserName);
        await _emailInput.FillAsync(ValidNonExistingEmail);
        await _passwordInput.FillAsync(ValidPassword);
        
        await _submitButton.ClickAsync();
        
        await Expect(_topbarLoginLink).ToBeVisibleAsync();
        await Expect(_topbarSignupLink).ToBeVisibleAsync();
        await Expect(_topbarUsername).Not.ToBeVisibleAsync();
        await Expect(_topbarLogoutButton).Not.ToBeVisibleAsync();
        
        await Expect(_signupError).ToBeVisibleAsync();
        await Expect(_signupError.Locator("li")).ToHaveCountAsync(1);
        await Expect(_signupError).ToContainTextAsync(ExistingUserNameError);       
    }
}