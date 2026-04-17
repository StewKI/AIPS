using AipsE2ETests.Abstract;
using Microsoft.Playwright;

namespace AipsE2ETests.Features.Auth;

[TestFixture]
public class LogOutTests : AuthorizedTestBase
{
    private ILocator _topbarLoginLink = null!;
    private ILocator _topbarSignupLink = null!;
    
    private ILocator _topbarUsername = null!;
    private ILocator _topbarLogoutButton = null!;

    private ILocator _logoutModal = null!;
    private ILocator _logoutModalConfirmButton = null!;
    private ILocator _logoutModalCancelButton = null!;
    private ILocator _logoutModalCloseButton = null!;
    
    [SetUp]
    public void LogOutTestsSetUp()
    {
        _topbarLoginLink = Page.GetByTestId("topbar-login-link");
        _topbarSignupLink = Page.GetByTestId("topbar-signup-link");
        
        _topbarUsername = Page.GetByTestId("topbar-username");
        _topbarLogoutButton = Page.GetByTestId("topbar-logout-button");
        
        _logoutModal = Page.GetByTestId("logout-confirm-modal");
        _logoutModalConfirmButton = _logoutModal.GetByTestId("logout-modal-confirm-button");
        _logoutModalCancelButton = _logoutModal.GetByTestId("logout-modal-cancel-button");
        _logoutModalCloseButton = _logoutModal.GetByTestId("logout-modal-close-button");
    }

    [Test]
    public async Task User_Can_Log_Out()
    {
        await Expect(_topbarLoginLink).Not.ToBeVisibleAsync();
        await Expect(_topbarSignupLink).Not.ToBeVisibleAsync();
        await Expect(_topbarUsername).ToBeVisibleAsync();
        await Expect(_topbarLogoutButton).ToBeVisibleAsync();
        
        await _topbarLogoutButton.ClickAsync();
        
        await Expect(_logoutModal).ToBeVisibleAsync();
        await Expect(_logoutModalConfirmButton).ToBeVisibleAsync();
        await Expect(_logoutModalCancelButton).ToBeVisibleAsync();
        await Expect(_logoutModalCloseButton).ToBeVisibleAsync();
        
        await _logoutModalConfirmButton.ClickAsync();
        
        await Expect(_topbarLoginLink).ToBeVisibleAsync();
        await Expect(_topbarSignupLink).ToBeVisibleAsync();
        await Expect(_topbarUsername).Not.ToBeVisibleAsync();
        await Expect(_topbarLogoutButton).Not.ToBeVisibleAsync();
    }
    
    [Test]
    public async Task User_Closes_Log_Out()
    {
        await Expect(_topbarLoginLink).Not.ToBeVisibleAsync();
        await Expect(_topbarSignupLink).Not.ToBeVisibleAsync();
        await Expect(_topbarUsername).ToBeVisibleAsync();
        await Expect(_topbarLogoutButton).ToBeVisibleAsync();
        
        await _topbarLogoutButton.ClickAsync();
        
        await Expect(_logoutModal).ToBeVisibleAsync();
        await Expect(_logoutModalConfirmButton).ToBeVisibleAsync();
        await Expect(_logoutModalCancelButton).ToBeVisibleAsync();
        await Expect(_logoutModalCloseButton).ToBeVisibleAsync();
        
        await _logoutModalCloseButton.ClickAsync();
        
        await Expect(_topbarLoginLink).Not.ToBeVisibleAsync();
        await Expect(_topbarSignupLink).Not.ToBeVisibleAsync();
        await Expect(_topbarUsername).ToBeVisibleAsync();
        await Expect(_topbarLogoutButton).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task User_Cancels_Log_Out()
    {
        await Expect(_topbarLoginLink).Not.ToBeVisibleAsync();
        await Expect(_topbarSignupLink).Not.ToBeVisibleAsync();
        await Expect(_topbarUsername).ToBeVisibleAsync();
        await Expect(_topbarLogoutButton).ToBeVisibleAsync();
        
        await _topbarLogoutButton.ClickAsync();
        
        await Expect(_logoutModal).ToBeVisibleAsync();
        await Expect(_logoutModalConfirmButton).ToBeVisibleAsync();
        await Expect(_logoutModalCancelButton).ToBeVisibleAsync();
        await Expect(_logoutModalCloseButton).ToBeVisibleAsync();
        
        await _logoutModalCancelButton.ClickAsync();
        
        await Expect(_topbarLoginLink).Not.ToBeVisibleAsync();
        await Expect(_topbarSignupLink).Not.ToBeVisibleAsync();
        await Expect(_topbarUsername).ToBeVisibleAsync();
        await Expect(_topbarLogoutButton).ToBeVisibleAsync();
    }
}