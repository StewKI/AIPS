using System.Text.RegularExpressions;
using AipsCore.Domain.Models.Whiteboard.Enums;
using AipsE2ETests.Abstract;
using Microsoft.Playwright;

namespace AipsE2ETests.Features.CreateWhiteboard;

[TestFixture]
public class CreateWhiteboardTests : AuthorizedTestBase
{
    private const string ValidTitle = "Test Whiteboard";
    private const string ValidMaxParticipants = "10";   
    
    private const string InvalidTitle = "X";
    private const string InvalidMaxParticipants = "-5";
    
    private ILocator _openCreateWhiteboardDialogButton = null!;

    private ILocator _createWhiteboardDialog = null!;

    private ILocator _createWhiteboardDialogTitle = null!;
    
    private ILocator _createWhiteboardDialogTitleInput = null!;
    private ILocator _createWhiteboardDialogJoinPolicySelect = null!;
    private ILocator _createWhiteboardDialogMaxParticipantsInput = null!;

    private ILocator _createWhiteboardDialogCloseButton = null!;
    private ILocator _createWhiteboardDialogCancelButton = null!;
    private ILocator _createWhiteboardDialogSubmitButton = null!;

    private ILocator _createWhiteboardDialogError = null!;

    private ILocator _whiteboardView = null!;

    [SetUp]
    public void CreateWhiteboardTestsSetUp()
    {
        _openCreateWhiteboardDialogButton = Page.GetByTestId("create-whiteboard-open-button");
        _createWhiteboardDialog = Page.GetByTestId("create-whiteboard-dialog");
        
        _createWhiteboardDialogTitle = _createWhiteboardDialog.GetByTestId("create-whiteboard-dialog-title");
        
        _createWhiteboardDialogTitleInput = _createWhiteboardDialog.GetByTestId("create-whiteboard-dialog-title-input");
        _createWhiteboardDialogJoinPolicySelect = _createWhiteboardDialog.GetByTestId("create-whiteboard-dialog-join-policy-select");
        _createWhiteboardDialogMaxParticipantsInput = _createWhiteboardDialog.GetByTestId("create-whiteboard-dialog-max-participants-input");
        
        _createWhiteboardDialogCloseButton = _createWhiteboardDialog.GetByTestId("create-whiteboard-dialog-close-button");
        _createWhiteboardDialogCancelButton = _createWhiteboardDialog.GetByTestId("create-whiteboard-dialog-cancel-button");
        _createWhiteboardDialogSubmitButton = _createWhiteboardDialog.GetByTestId("create-whiteboard-dialog-submit-button");
        
        _createWhiteboardDialogError = _createWhiteboardDialog.GetByTestId("create-whiteboard-dialog-error");      
        
        _whiteboardView = Page.GetByTestId("whiteboard-view");  
    }
    
    [Test]
    public async Task User_Can_Open_Creation_Dialog()
    {
        await Expect(_openCreateWhiteboardDialogButton).ToBeVisibleAsync();

        await _openCreateWhiteboardDialogButton.ClickAsync();
        
        await Expect(_createWhiteboardDialog).ToBeVisibleAsync();
        await Expect(_createWhiteboardDialogTitle).ToBeVisibleAsync();
        
        await Expect(_createWhiteboardDialogTitleInput).ToBeVisibleAsync();
        await Expect(_createWhiteboardDialogJoinPolicySelect).ToBeVisibleAsync();
        await Expect(_createWhiteboardDialogMaxParticipantsInput).ToBeVisibleAsync();
        
        await Expect(_createWhiteboardDialogCloseButton).ToBeVisibleAsync();
        await Expect(_createWhiteboardDialogCancelButton).ToBeVisibleAsync();
        await Expect(_createWhiteboardDialogSubmitButton).ToBeVisibleAsync();
        await Expect(_createWhiteboardDialogError).Not.ToBeVisibleAsync();       
    }
    
    [Test]
    public async Task User_Can_Close_Creation_Dialog()
    {
        await _openCreateWhiteboardDialogButton.ClickAsync();
        
        await _createWhiteboardDialogCloseButton.ClickAsync();
        
        await Expect(_createWhiteboardDialog).Not.ToBeVisibleAsync();      
    }

    [Test]
    public async Task User_Can_Cancel_Creation()
    {
        await _openCreateWhiteboardDialogButton.ClickAsync();
        
        await _createWhiteboardDialogCancelButton.ClickAsync();
        
        await Expect(_createWhiteboardDialog).Not.ToBeVisibleAsync();      
    }

    [Test]
    public async Task User_Can_Create_Whiteboard_With_Valid_Data()
    {
        await _openCreateWhiteboardDialogButton.ClickAsync();

        await _createWhiteboardDialogTitleInput.FillAsync(ValidTitle);
        await _createWhiteboardDialogJoinPolicySelect.SelectOptionAsync(((int)WhiteboardJoinPolicy.FreeToJoin).ToString());
        await _createWhiteboardDialogMaxParticipantsInput.FillAsync(ValidMaxParticipants);
        
        await _createWhiteboardDialogSubmitButton.ClickAsync();
        
        await Page.WaitForURLAsync(new Regex(@"/whiteboard/.*"));
    }
    
    [Test]
    public async Task User_Cannot_Create_Whiteboard_With_Invalid_Data()
    {
        await _openCreateWhiteboardDialogButton.ClickAsync();

        await _createWhiteboardDialogTitleInput.FillAsync(InvalidTitle);
        await _createWhiteboardDialogJoinPolicySelect.SelectOptionAsync(((int)WhiteboardJoinPolicy.FreeToJoin).ToString());
        await _createWhiteboardDialogMaxParticipantsInput.FillAsync(InvalidMaxParticipants);
        
        await _createWhiteboardDialogSubmitButton.ClickAsync();
        
        await Expect(_createWhiteboardDialogError).ToBeVisibleAsync();
    }
}