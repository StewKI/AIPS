using AipsCore.Application.Common.Message.ErrorMessage;
using AipsRT.Hubs;
using AipsRT.Model.Whiteboard;
using Microsoft.AspNetCore.SignalR;

namespace AipsRT.Services;

public class RtErrorHandleStrategy : IErrorMessageHandleStrategy
{
    private readonly IHubContext<WhiteboardHub> _hubContext;
    private readonly WhiteboardManager _whiteboardManager;

    public RtErrorHandleStrategy(IHubContext<WhiteboardHub> hubContext, WhiteboardManager whiteboardManager)
    {
        _hubContext = hubContext;
        _whiteboardManager = whiteboardManager;
    }
    
    public async Task Handle(ErrorMessage message, CancellationToken cancellationToken)
    {
        var activeUsers = _whiteboardManager.GetWhiteboard(message.WhiteboardId)!.ActiveUsers;
        
        await _whiteboardManager.LoadWhiteboard(message.WhiteboardId);
        
        var whiteboard = _whiteboardManager.GetWhiteboard(message.WhiteboardId)!;

        foreach (var user in activeUsers)
        {
            whiteboard.AddActiveUser(user);
        }
        
        await _hubContext.Clients
            .Group(whiteboard.WhiteboardId.ToString())
            .SendAsync("InitWhiteboard", whiteboard, cancellationToken);
    }
}