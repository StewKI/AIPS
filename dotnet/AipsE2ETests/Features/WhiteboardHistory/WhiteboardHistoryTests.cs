using System.Globalization;
using AipsCore.Domain.Models.Whiteboard.Enums;
using AipsE2ETests.Abstract;
using Microsoft.Playwright;

namespace AipsE2ETests.Features.WhiteboardHistory;

[TestFixture]
public class WhiteboardHistoryTests : AuthorizedTestBase
{
    private ILocator _whiteboardHistorySidebarOpenButton = null!;
    
    private ILocator _whiteboardHistorySidebar = null!;
    
    private ILocator _whiteboardHistoryTitle = null!;
    private ILocator _whiteboardHistorySidebarCloseButton = null!;
    private ILocator _whiteboardHistoryList = null!;
    private ILocator _whiteboardHistoryEmpty = null!;
    
    private ILocator _whiteboardHistoryItems = null!;
    
    private ILocator _whiteboardHistoryItemTitle = null!;
    private ILocator _whiteboardHistoryItemCreationDate = null!;
    private ILocator _whiteboardHistoryItemDeleteButton = null!;
    
    [SetUp]
    public async Task MyWhiteboardsTestsSetUp()
    {
        await Page.SetViewportSizeAsync(1280, 720); 
        
        _whiteboardHistorySidebarOpenButton = Page.GetByTestId("whiteboard-history-sidebar-open-button");
        
        _whiteboardHistorySidebar = Page.GetByTestId("whiteboard-history-sidebar");
        
        _whiteboardHistoryTitle = _whiteboardHistorySidebar.GetByTestId("whiteboard-history-sidebar-title");
        _whiteboardHistorySidebarCloseButton = _whiteboardHistorySidebar.GetByTestId("whiteboard-history-sidebar-close-button");
        _whiteboardHistoryList = _whiteboardHistorySidebar.GetByTestId("whiteboard-history-list");
        _whiteboardHistoryEmpty = _whiteboardHistorySidebar.GetByTestId("whiteboard-history-empty");
        
        _whiteboardHistoryItems = _whiteboardHistoryList.GetByTestId("whiteboard-history-item");
        
        _whiteboardHistoryItemTitle = _whiteboardHistoryItems.GetByTestId("whiteboard-history-item-title");
        _whiteboardHistoryItemCreationDate = _whiteboardHistoryItems.GetByTestId("whiteboard-history-item-creation-date");
        _whiteboardHistoryItemDeleteButton = _whiteboardHistoryItems.GetByTestId("whiteboard-history-item-delete-button");       
    }

    [Test]
    public async Task User_Can_Open_Whiteboard_History_Sidebar()
    {
        await Expect(_whiteboardHistorySidebarOpenButton).ToBeVisibleAsync();
        await Expect(_whiteboardHistorySidebar).Not.ToBeVisibleAsync();
        await Expect(_whiteboardHistorySidebarCloseButton).Not.ToBeVisibleAsync();
        await Expect(_whiteboardHistoryTitle).Not.ToBeVisibleAsync();       
        
        await _whiteboardHistorySidebarOpenButton.ClickAsync();
        
        await Expect(_whiteboardHistorySidebar).ToBeVisibleAsync();
        await Expect(_whiteboardHistorySidebarCloseButton).ToBeVisibleAsync();       
        await Expect(_whiteboardHistoryTitle).ToBeVisibleAsync();     
    }
    
    [Test]
    public async Task User_Can_Close_Whiteboard_History_Sidebar()
    {
        await _whiteboardHistorySidebarOpenButton.ClickAsync();
        
        await Expect(_whiteboardHistorySidebar).ToBeVisibleAsync();
        await Expect(_whiteboardHistorySidebarCloseButton).ToBeVisibleAsync();     
        
        await _whiteboardHistorySidebarCloseButton.ClickAsync();
        
        await Expect(_whiteboardHistorySidebarOpenButton).ToBeVisibleAsync();
        await Expect(_whiteboardHistorySidebar).Not.ToBeVisibleAsync();       
        await Expect(_whiteboardHistorySidebarCloseButton).Not.ToBeVisibleAsync();       
    }

    [Test]
    public async Task User_Can_See_No_Whiteboards_Yet()
    {
        await _whiteboardHistorySidebarOpenButton.ClickAsync();
        
        await Expect(_whiteboardHistoryList).Not.ToBeVisibleAsync();
        await Expect(_whiteboardHistoryEmpty).ToBeVisibleAsync();
        await Expect(_whiteboardHistoryItems).ToHaveCountAsync(0);
    }
    
    
    [Test]
    public async Task User_Can_See_His_Whiteboard_History()
    {
        const string title = "Test Whiteboard";
        
        await TestEnvironment.CreateWhiteboardForUser(DefaultEmail, DefaultPassword, title, WhiteboardJoinPolicy.FreeToJoin, 10);
        await Page.ReloadAsync();
        
        await _whiteboardHistorySidebarOpenButton.ClickAsync();
        
        await Expect(_whiteboardHistoryEmpty).Not.ToBeVisibleAsync();
        await Expect(_whiteboardHistoryList).ToBeVisibleAsync();
        await Expect(_whiteboardHistoryItems).ToHaveCountAsync(1);  
        
        await Expect(_whiteboardHistoryItemTitle.First).ToContainTextAsync(title);
        await Expect(_whiteboardHistoryItemCreationDate.First).ToContainTextAsync(DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
        await Expect(_whiteboardHistoryItemDeleteButton.First).ToBeVisibleAsync();       
    }
    
    [TestCase(10)]
    public async Task User_Can_Scroll_Through_His_Whiteboard_History(int whiteboardCount)
    {
        await TestEnvironment.CreateMultipleWhiteboardsForUser(DefaultEmail, DefaultPassword, whiteboardCount);
        await Page.ReloadAsync();
        
        await _whiteboardHistorySidebarOpenButton.ClickAsync();
        
        await Expect(_whiteboardHistoryItems).ToHaveCountAsync(whiteboardCount); 
        
        await Expect(_whiteboardHistoryItems.First).ToBeVisibleAsync();
        
        await _whiteboardHistoryItems.Last.ScrollIntoViewIfNeededAsync();
        
        await Expect(_whiteboardHistoryItems.Last).ToBeVisibleAsync();
    }
}