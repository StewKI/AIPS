using AipsRT.Model.Whiteboard;
using AipsRT.Model.Whiteboard.Shapes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AipsRT.Hubs;

[Authorize]
public class WhiteboardHub : Hub
{
    private readonly WhiteboardManager _whiteboardManager;

    public WhiteboardHub(WhiteboardManager whiteboardManager)
    {
        _whiteboardManager = whiteboardManager;
    }

    public async Task JoinWhiteboard(Guid whiteboardId)
    {
        if (!_whiteboardManager.WhiteboardExists(whiteboardId))
            await _whiteboardManager.AddWhiteboard(whiteboardId);
        
        await Groups.AddToGroupAsync(Context.ConnectionId, whiteboardId.ToString());
        
        var state = _whiteboardManager.GetWhiteboard(whiteboardId)!;
        
        _whiteboardManager.AddUserToWhiteboard(Guid.Parse(Context.UserIdentifier!), whiteboardId);
        
        await Clients.Caller.SendAsync("InitWhiteboard", state);
        await Clients.GroupExcept(whiteboardId.ToString(), Context.ConnectionId)
            .SendAsync("Joined", Context.UserIdentifier!);
    }

    public async Task LeaveWhiteboard(Guid whiteboardId)
    {
        await Clients.GroupExcept(whiteboardId.ToString(), Context.ConnectionId)
            .SendAsync("Leaved", Context.UserIdentifier!);
    }

    public async Task AddRectangle(Rectangle rectangle)
    {
        var whiteboard = _whiteboardManager.GetWhiteboardForUser(Guid.Parse(Context.UserIdentifier!))!;
        
        whiteboard.AddRectangle(rectangle);
        
        await Clients.GroupExcept(whiteboard.WhiteboardId.ToString(), Context.ConnectionId)
            .SendAsync("AddedRectangle", rectangle);
    }
}