using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Common.Message.MoveShape;
using AipsCore.Application.Models.Shape.Command.MoveShape;
using AipsRT.Model.Whiteboard;
using AipsRT.Model.Whiteboard.Shapes;
using AipsRT.Model.Whiteboard.Structs;
using AipsRT.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AipsRT.Hubs;

[Authorize]
public class WhiteboardHub : Hub
{
    private readonly WhiteboardManager _whiteboardManager;
    private readonly IMessagingService _messagingService;

    public WhiteboardHub(WhiteboardManager whiteboardManager, IMessagingService messagingService)
    {
        _whiteboardManager = whiteboardManager;
        _messagingService = messagingService;
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

    private Guid CurrentUserId => Guid.Parse(Context.UserIdentifier!);
    private Whiteboard CurrentWhiteboard => _whiteboardManager.GetWhiteboardForUser(CurrentUserId)!;

    private async Task ResetCurrentUser()
    {
        await Clients.Caller.SendAsync("InitWhiteboard", CurrentWhiteboard);
    }

    private async Task SendToOthers(string methodName, object? arg)
    {
        await Clients.GroupExcept(CurrentWhiteboard.WhiteboardId.ToString(), Context.ConnectionId)
            .SendAsync(methodName, arg);
    }
    
    public async Task AddRectangle(Rectangle rectangle)
    {
        rectangle.OwnerId = CurrentUserId;
        var whiteboard = CurrentWhiteboard;
        
        await _messagingService.CreatedRectangle(whiteboard.WhiteboardId, rectangle);
        
        whiteboard.AddRectangle(rectangle);
        
        await SendToOthers("AddedRectangle", rectangle);
    }

    public async Task AddArrow(Arrow arrow)
    {
        arrow.OwnerId = CurrentUserId;
        var whiteboard = CurrentWhiteboard;
        
        await _messagingService.CreatedArrow(whiteboard.WhiteboardId, arrow);
        
        whiteboard.AddArrow(arrow);
        
        await SendToOthers("AddedArrow", arrow);
    }

    public async Task AddLine(Line line)
    {
        line.OwnerId = CurrentUserId;
        var whiteboard = CurrentWhiteboard;

        await _messagingService.CreateLine(whiteboard.WhiteboardId, line);
        
        whiteboard.AddLine(line);
        
        await SendToOthers("AddedLine", line);
    }

    public async Task AddTextShape(TextShape textShape)
    {
        textShape.OwnerId = CurrentUserId;
        var whiteboard = CurrentWhiteboard;
        
        await _messagingService.CreateTextShape(whiteboard.WhiteboardId, textShape);
        
        whiteboard.AddTextShape(textShape);
        
        await SendToOthers("AddedTextShape", textShape);
    }

    public async Task MoveShape(MoveShapeCommand moveShape)
    {
        var whiteboard = CurrentWhiteboard;
        
        var shape = whiteboard.Shapes.Find(s => s.Id.ToString() == moveShape.ShapeId);

        if (shape is null || shape.OwnerId != CurrentUserId)
        {
            await ResetCurrentUser();
            return;
        }
        
        shape.Move(new Position(moveShape.NewPositionX, moveShape.NewPositionY));
        
        await SendToOthers("MovedShape", moveShape);
    }
    
    public async Task PlaceShape(MoveShapeCommand moveShape)
    {
        await MoveShape(moveShape);
        
        await _messagingService.MoveShape(moveShape);
    }
}