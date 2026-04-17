using System.Text.RegularExpressions;
using AipsCore.Domain.Models.Whiteboard.Enums;
using AipsE2ETests.Abstract;
using Microsoft.Playwright;

namespace AipsE2ETests.Features.JoinWhiteboard;

[TestFixture]
public partial class JoinWhiteboardTests : AuthorizedTestBase
{
    private const string OtherUserName = "OtherUser";
    private const string OtherUserEmail = "other@email.com";
    private const string OtherUserPassword = "Password123!";   
    
    private ILocator _joinWithCodeInput = null!;
    private ILocator _joinWithCodeSubmitButton = null!;
    
    private ILocator _joinWithCodeError = null!;
    
    private ILocator _whiteboardView = null!;
    private ILocator _whiteboardViewLeaveButton = null!;

    private ILocator _whiteboardViewWaitingRoom = null!;
    private ILocator _whiteboardViewWaitingRoomCancelButton = null!;

    private ILocator _whiteboardHistorySidebarOpenButton = null!;
    private ILocator _whiteboardHistorySidebar = null!;
    private ILocator _whiteboardHistoryList = null!;
    private ILocator _whiteboardHistoryItems = null!;

    private ILocator _recentWhiteboardsSidebarOpenButton = null!;
    private ILocator _recentWhiteboardsSidebar = null!;
    private ILocator _recentWhiteboardsList = null!;
    private ILocator _recentWhiteboardsItems = null!;
    
    [SetUp]
    public void JoinWhiteboardTestsSetUp()
    {
        _whiteboardView = Page.GetByTestId("whiteboard-view");
        
        _whiteboardViewLeaveButton = _whiteboardView.GetByTestId("whiteboard-view-leave-button");
        
        _joinWithCodeInput = Page.GetByTestId("join-with-code-input");
        _joinWithCodeSubmitButton = Page.GetByTestId("join-with-code-submit-button");
        
        _joinWithCodeError = Page.GetByTestId("join-with-code-error");   
        
        _whiteboardViewWaitingRoom = Page.GetByTestId("whiteboard-view-waiting-room");
        _whiteboardViewWaitingRoomCancelButton = _whiteboardViewWaitingRoom.GetByTestId("whiteboard-view-waiting-room-cancel-button");      
        
        _whiteboardHistorySidebarOpenButton = Page.GetByTestId("whiteboard-history-sidebar-open-button");
        _whiteboardHistorySidebar = Page.GetByTestId("whiteboard-history-sidebar");
        _whiteboardHistoryList = _whiteboardHistorySidebar.GetByTestId("whiteboard-history-list");
        _whiteboardHistoryItems = _whiteboardHistoryList.GetByTestId("whiteboard-history-item");      
        
        _recentWhiteboardsSidebarOpenButton = Page.GetByTestId("recent-whiteboards-sidebar-open-button");
        _recentWhiteboardsSidebar = Page.GetByTestId("recent-whiteboards-sidebar");
        _recentWhiteboardsList = _recentWhiteboardsSidebar.GetByTestId("recent-whiteboards-list");   
        _recentWhiteboardsItems = _recentWhiteboardsList.GetByTestId("recent-whiteboards-item");      
    }

    [Test]
    public async Task User_Can_Join_His_Whiteboard_With_Valid_Code()
    {
        var whiteboardId = await TestEnvironment.CreateWhiteboardForUser(DefaultEmail, DefaultPassword, "Test", WhiteboardJoinPolicy.FreeToJoin, 10);
        await Page.ReloadAsync();

        var code = await TestEnvironment.GetWhiteboardCodeFromId(DefaultEmail, DefaultPassword, whiteboardId);

        await _joinWithCodeInput.FillAsync(code);

        await _joinWithCodeSubmitButton.ClickAsync();
        
        await Page.WaitForURLAsync(WhiteboardViewUrl());
    }

    [Test]
    public async Task User_Can_Join_His_Whiteboard_Through_Whiteboard_History()
    {
        _ = await TestEnvironment.CreateWhiteboardForUser(DefaultEmail, DefaultPassword, "Test", WhiteboardJoinPolicy.FreeToJoin, 10);
        await Page.ReloadAsync();
        
        await _whiteboardHistorySidebarOpenButton.ClickAsync();

        await _whiteboardHistoryItems.First.ClickAsync();
        
        await Page.WaitForURLAsync(WhiteboardViewUrl());
    }

    [Test]
    public async Task User_Can_Join_Others_Whiteboard_With_Valid_Code()
    {
        await TestEnvironment.CreateUser(OtherUserName, OtherUserEmail, OtherUserPassword);
        var whiteboardId = await TestEnvironment.CreateWhiteboardForUser(OtherUserEmail, OtherUserPassword, "Test", WhiteboardJoinPolicy.FreeToJoin, 10);
        await Page.ReloadAsync();
        
        var code = await TestEnvironment.GetWhiteboardCodeFromId(OtherUserEmail, OtherUserPassword, whiteboardId);
        
        await _joinWithCodeInput.FillAsync(code);
        
        await _joinWithCodeSubmitButton.ClickAsync();
        
        await Page.WaitForURLAsync(WhiteboardViewUrl());       
    }

    [Test]
    public async Task User_Cannot_Join_Others_Whiteboard_With_Invalid_Code()
    {
        await TestEnvironment.CreateUser(OtherUserName, OtherUserEmail, OtherUserPassword);
        var whiteboardId = await TestEnvironment.CreateWhiteboardForUser(OtherUserEmail, OtherUserPassword, "Test", WhiteboardJoinPolicy.FreeToJoin, 10);
        await Page.ReloadAsync();
        
        var code = await TestEnvironment.GetWhiteboardCodeFromId(OtherUserEmail, OtherUserPassword, whiteboardId);
        
        await _joinWithCodeInput.FillAsync(MakeInvalidCode(code));
        
        await _joinWithCodeSubmitButton.ClickAsync();
        
        await Expect(_joinWithCodeError).ToBeVisibleAsync();      
    }
    
    [Test]
    public async Task User_Can_Join_Others_Whiteboard_Through_Recent_Whiteboards()
    {
        const string title = "Other Test Whiteboard";
        
        await TestEnvironment.CreateUser(OtherUserName, OtherUserEmail, OtherUserPassword);
        var whiteboardId = await TestEnvironment.CreateWhiteboardForUser(OtherUserEmail, OtherUserPassword, title, WhiteboardJoinPolicy.FreeToJoin, 10);
        await TestEnvironment.CreateWhiteboardMembership(DefaultEmail, DefaultPassword, whiteboardId);
        await Page.ReloadAsync();
        
        await _recentWhiteboardsSidebarOpenButton.ClickAsync();
        
        await _recentWhiteboardsItems.First.ClickAsync();
        
        await Page.WaitForURLAsync(WhiteboardViewUrl());      
    }

    [Test]
    public async Task User_Can_Leave_Whiteboard()
    {
        _ = await TestEnvironment.CreateWhiteboardForUser(DefaultEmail, DefaultPassword, "Test", WhiteboardJoinPolicy.FreeToJoin, 10);
        await Page.ReloadAsync();
        
        await _whiteboardHistorySidebarOpenButton.ClickAsync();

        await _whiteboardHistoryItems.First.ClickAsync();
        
        await _whiteboardViewLeaveButton.ClickAsync();
        
        await Page.WaitForURLAsync(BaseUrl);      
    }

    [Test]
    public async Task User_Can_Request_To_Join_Whiteboard()
    {
        await TestEnvironment.CreateUser(OtherUserName, OtherUserEmail, OtherUserPassword);
        var whiteboardId = await TestEnvironment.CreateWhiteboardForUser(OtherUserEmail, OtherUserPassword, "Test", WhiteboardJoinPolicy.RequestToJoin, 10);
        await Page.ReloadAsync();
        
        var code = await TestEnvironment.GetWhiteboardCodeFromId(OtherUserEmail, OtherUserPassword, whiteboardId);
        
        await _joinWithCodeInput.FillAsync(code);
        
        await _joinWithCodeSubmitButton.ClickAsync();
        
        await Page.WaitForURLAsync(WhiteboardViewUrl());  
        
        await Expect(_whiteboardViewWaitingRoom).ToBeVisibleAsync();    
        await Expect(_whiteboardViewWaitingRoomCancelButton).ToBeVisibleAsync();    
    }
    
    [Test]
    public async Task User_Cannot_Join_Private_Whiteboard()
    {
        await TestEnvironment.CreateUser(OtherUserName, OtherUserEmail, OtherUserPassword);
        var whiteboardId = await TestEnvironment.CreateWhiteboardForUser(OtherUserEmail, OtherUserPassword, "Test", WhiteboardJoinPolicy.Private, 10);
        await Page.ReloadAsync();
        
        var code = await TestEnvironment.GetWhiteboardCodeFromId(OtherUserEmail, OtherUserPassword, whiteboardId);
        
        await _joinWithCodeInput.FillAsync(code);
        
        await _joinWithCodeSubmitButton.ClickAsync();
        
        await Expect(_joinWithCodeError).ToBeVisibleAsync();  
    }   
    
    private string MakeInvalidCode(string validCode)
    {
        char first = validCode[0];
    
        char newChar = first == '0' ? '1' : '0';

        return newChar + validCode[1..];
    }

    [GeneratedRegex(@"/whiteboard/.*")]
    private static partial Regex WhiteboardViewUrl();
}