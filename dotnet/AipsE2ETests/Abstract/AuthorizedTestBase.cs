using Microsoft.Playwright;

namespace AipsE2ETests.Abstract;

public abstract class AuthorizedTestBase : PlaywrightTestBase
{
    protected const string DefaultUserName = "TestUser";
    protected const string DefaultEmail = "test@email.com";
    protected const string DefaultPassword = "Password123!";
    
    private const string SignupPageUrl = $"{BaseUrl}/signup";

    private ILocator _form = null!;
    private ILocator _usernameInput = null!;
    private ILocator _emailInput = null!;
    private ILocator _passwordInput = null!;
    private ILocator _submitButton = null!;
    
    [SetUp]
    public async Task AuthorizedTestSetUp()
    {
        await Page.GotoAsync(SignupPageUrl, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        });
        
        _form = Page.GetByTestId("signup-form");
        _usernameInput = _form.GetByTestId("signup-username-input");
        _emailInput = _form.GetByTestId("signup-email-input");
        _passwordInput = _form.GetByTestId("signup-password-input");
        _submitButton = _form.GetByTestId("signup-submit-button");
        
        await _usernameInput.FillAsync(DefaultUserName);
        await _emailInput.FillAsync(DefaultEmail);
        await _passwordInput.FillAsync(DefaultPassword);
        
        await _submitButton.ClickAsync();
        
        await Page.WaitForURLAsync(BaseUrl);       
    }
}