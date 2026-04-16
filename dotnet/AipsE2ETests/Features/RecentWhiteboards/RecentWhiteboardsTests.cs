using System.Globalization;
using AipsCore.Domain.Models.Whiteboard.Enums;
using AipsE2ETests.Abstract;
using Microsoft.Playwright;

namespace AipsE2ETests.Features.RecentWhiteboards;

[TestFixture]
public class RecentWhiteboardsTests : AuthorizedTestBase
{
    private const string OtherUserName = "OtherUser";
    private const string OtherUserEmail = "other@user.com";
    private const string OtherUserPassword = "Password123!";
    
    private ILocator _recentWhiteboardsSidebarOpenButton = null!;
    
    private ILocator _recentWhiteboardsSidebar = null!;
    
    private ILocator _recentWhiteboardsTitle = null!;
    private ILocator _recentWhiteboardsSidebarCloseButton = null!;
    private ILocator _recentWhiteboardsList = null!;
    private ILocator _recentWhiteboardsEmpty = null!;
    
    private ILocator _recentWhiteboardsItems = null!;
    
    private ILocator _recentWhiteboardsItemTitle = null!;
    private ILocator _recentWhiteboardsItemCreationDate = null!;

    [SetUp]
    public async Task RecentWhiteboardsTestsSetUp()
    {
        await Page.SetViewportSizeAsync(1280, 720);
        
        _recentWhiteboardsSidebarOpenButton = Page.GetByTestId("recent-whiteboards-sidebar-open-button");
        
        _recentWhiteboardsSidebar = Page.GetByTestId("recent-whiteboards-sidebar");
        
        _recentWhiteboardsTitle = _recentWhiteboardsSidebar.GetByTestId("recent-whiteboards-sidebar-title");
        _recentWhiteboardsSidebarCloseButton = _recentWhiteboardsSidebar.GetByTestId("recent-whiteboards-sidebar-close-button");
        _recentWhiteboardsList = _recentWhiteboardsSidebar.GetByTestId("recent-whiteboards-list");
        _recentWhiteboardsEmpty = _recentWhiteboardsSidebar.GetByTestId("recent-whiteboards-empty");
        
        _recentWhiteboardsItems = _recentWhiteboardsList.GetByTestId("recent-whiteboards-item");
        
        _recentWhiteboardsItemTitle = _recentWhiteboardsItems.GetByTestId("recent-whiteboards-item-title");
        _recentWhiteboardsItemCreationDate = _recentWhiteboardsItems.GetByTestId("recent-whiteboards-item-creation-date");
    }
    
    [Test]
    public async Task User_Can_Open_Recent_Whiteboards_Sidebar()
    {
        await Expect(_recentWhiteboardsSidebarOpenButton).ToBeVisibleAsync();
        await Expect(_recentWhiteboardsSidebar).Not.ToBeVisibleAsync();
        await Expect(_recentWhiteboardsSidebarCloseButton).Not.ToBeVisibleAsync();
        await Expect(_recentWhiteboardsTitle).Not.ToBeVisibleAsync();       
        
        await _recentWhiteboardsSidebarOpenButton.ClickAsync();
        
        await Expect(_recentWhiteboardsSidebar).ToBeVisibleAsync();
        await Expect(_recentWhiteboardsSidebarCloseButton).ToBeVisibleAsync();       
        await Expect(_recentWhiteboardsTitle).ToBeVisibleAsync();     
    }
    
    [Test]
    public async Task User_Can_Close_Recent_Whiteboards_Sidebar()
    {
        await _recentWhiteboardsSidebarOpenButton.ClickAsync();
        
        await Expect(_recentWhiteboardsSidebar).ToBeVisibleAsync();
        await Expect(_recentWhiteboardsSidebarCloseButton).ToBeVisibleAsync();     
        
        await _recentWhiteboardsSidebarCloseButton.ClickAsync();
        
        await Expect(_recentWhiteboardsSidebarOpenButton).ToBeVisibleAsync();
        await Expect(_recentWhiteboardsSidebar).Not.ToBeVisibleAsync();       
        await Expect(_recentWhiteboardsSidebarCloseButton).Not.ToBeVisibleAsync();       
    }
    
    [Test]
    public async Task User_Can_See_No_Recent_Whiteboards_Yet()
    {
        await _recentWhiteboardsSidebarOpenButton.ClickAsync();
        
        await Expect(_recentWhiteboardsList).Not.ToBeVisibleAsync();
        await Expect(_recentWhiteboardsEmpty).ToBeVisibleAsync();
        await Expect(_recentWhiteboardsItems).ToHaveCountAsync(0);
    }
    
    [Test]
    public async Task User_Can_See_His_Recent_Whiteboards()
    {
        const string title = "Other Test Whiteboard";
        
        await TestEnvironment.CreateUser(OtherUserName, OtherUserEmail, OtherUserPassword);
        var whiteboardId = await TestEnvironment.CreateWhiteboardForUser(OtherUserEmail, OtherUserPassword, title, WhiteboardJoinPolicy.FreeToJoin, 10);
        await TestEnvironment.CreateWhiteboardMembership(DefaultEmail, DefaultPassword, whiteboardId);
        await Page.ReloadAsync();
        
        await _recentWhiteboardsSidebarOpenButton.ClickAsync();
        
        await Expect(_recentWhiteboardsEmpty).Not.ToBeVisibleAsync();
        await Expect(_recentWhiteboardsList).ToBeVisibleAsync();
        await Expect(_recentWhiteboardsItems).ToHaveCountAsync(1);  
        
        await Expect(_recentWhiteboardsItemTitle.First).ToContainTextAsync(title);
        await Expect(_recentWhiteboardsItemCreationDate.First).ToContainTextAsync(DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
    }
    
    [TestCase(10)]
    public async Task User_Can_Scroll_Through_His_Whiteboard_History(int whiteboardCount)
    {
        await TestEnvironment.CreateUser(OtherUserName, OtherUserEmail, OtherUserPassword);
        
        var otherWhiteboards = await TestEnvironment.CreateMultipleWhiteboardsForUser(OtherUserEmail, OtherUserPassword, whiteboardCount);
        await TestEnvironment.CreateMultipleWhiteboardMemberships(DefaultEmail, DefaultPassword, otherWhiteboards);
        await Page.ReloadAsync();
        
        await _recentWhiteboardsSidebarOpenButton.ClickAsync();
        
        await Expect(_recentWhiteboardsItems).ToHaveCountAsync(whiteboardCount); 
        
        await Expect(_recentWhiteboardsItems.First).ToBeVisibleAsync();
        
        await _recentWhiteboardsItems.Last.ScrollIntoViewIfNeededAsync();
        
        await Expect(_recentWhiteboardsItems.Last).ToBeVisibleAsync();
    }
}