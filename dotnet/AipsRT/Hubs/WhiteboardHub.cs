using AipsCore.Application.Abstract;
using AipsCore.Application.Models.Shape.Command.MoveShape;
using AipsCore.Application.Models.Whiteboard.Command.AcceptUserRequestToJoin;
using AipsCore.Application.Models.Whiteboard.Command.RejectUserRequestToJoin;
using AipsCore.Application.Models.Whiteboard.Query.GetMembershipStatus;
using AipsCore.Domain.Models.WhiteboardMembership.Enums;
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
    private readonly IDispatcher _dispatcher;

    public WhiteboardHub(WhiteboardManager whiteboardManager, IMessagingService messagingService, IDispatcher dispatcher)
    {
        _whiteboardManager = whiteboardManager;
        _messagingService = messagingService;
        _dispatcher = dispatcher;
    }

    public async Task JoinWhiteboard(Guid whiteboardId)
    {
        if (!_whiteboardManager.WhiteboardExists(whiteboardId))
        {
            await _whiteboardManager.AddWhiteboard(whiteboardId);
        }
        
        await Groups.AddToGroupAsync(Context.ConnectionId, whiteboardId.ToString());

        var whiteboard = _whiteboardManager.GetWhiteboard(whiteboardId)!;
        
        var userId = CurrentUserId;
        var ownerId = whiteboard.OwnerId;
        
        WhiteboardMembershipStatus status;

        if (userId == ownerId)
        {
            status = WhiteboardMembershipStatus.Accepted;
        }
        else
        {
            status = await _dispatcher.Execute(new GetMembershipStatusQuery(whiteboardId.ToString(), userId.ToString()));
        }
        
        if (status == WhiteboardMembershipStatus.Accepted)
        {
            _whiteboardManager.AddUserToWhiteboard(userId, whiteboardId);
            whiteboard.AcceptUser(userId);
            
            var state = _whiteboardManager.GetWhiteboard(whiteboardId)!;
            await Clients.Caller.SendAsync("InitWhiteboard", state);
            
            await Clients.GroupExcept(whiteboardId.ToString(), Context.ConnectionId).SendAsync("Joined", Context.UserIdentifier!);
        }
        else
        {
            _whiteboardManager.AddPendingUser(userId, whiteboardId);
            
            await Clients.Caller.SendAsync("WaitingForApproval", userId.ToString());

            await Clients.User(ownerId.ToString()).SendAsync("UserWaitingForApproval", userId.ToString());
        }
    }

    public async Task AcceptUser(Guid targetUserId)
    {
        var whiteboard = CurrentWhiteboard;

        await _messagingService.AcceptedUser(new AcceptUserRequestToJoinCommand(whiteboard.WhiteboardId.ToString(), targetUserId.ToString()));
        
        _whiteboardManager.MovePendingToAccepted(targetUserId, whiteboard.WhiteboardId);
        
        await Clients.User(targetUserId.ToString()).SendAsync("Accepted");
        await Clients.User(targetUserId.ToString()).SendAsync("InitWhiteboard", whiteboard);
    }
    
    public async Task RejectUser(Guid targetUserId)
    {
        var whiteboard = CurrentWhiteboard;

        await _messagingService.RejectedUser(new RejectUserRequestToJoinCommand(whiteboard.WhiteboardId.ToString(), targetUserId.ToString()));
        
        _whiteboardManager.RemovePendingUser(targetUserId, whiteboard.WhiteboardId);
        
        await Clients.User(targetUserId.ToString()).SendAsync("Rejected");
    }
    
    public async Task CancelJoinRequest()
    {
        var userId = CurrentUserId;
        var whiteboard = _whiteboardManager.GetWhiteboardForUser(userId);

        if (whiteboard != null)
        {
            _whiteboardManager.RemovePendingUser(userId, whiteboard.WhiteboardId);

            await Clients.User(whiteboard.OwnerId.ToString())
                .SendAsync("UserCanceledJoinRequest", userId.ToString());
        }
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
        if (!_whiteboardManager.IsAccepted(CurrentUserId)) return;
        
        rectangle.OwnerId = CurrentUserId;
        var whiteboard = CurrentWhiteboard;
        
        await _messagingService.CreatedRectangle(whiteboard.WhiteboardId, rectangle);
        
        whiteboard.AddRectangle(rectangle);
        
        await SendToOthers("AddedRectangle", rectangle);
    }

    public async Task AddArrow(Arrow arrow)
    {
        if (!_whiteboardManager.IsAccepted(CurrentUserId)) return;
        
        arrow.OwnerId = CurrentUserId;
        var whiteboard = CurrentWhiteboard;
        
        await _messagingService.CreatedArrow(whiteboard.WhiteboardId, arrow);
        
        whiteboard.AddArrow(arrow);
        
        await SendToOthers("AddedArrow", arrow);
    }

    public async Task AddLine(Line line)
    {
        if (!_whiteboardManager.IsAccepted(CurrentUserId)) return;
        
        line.OwnerId = CurrentUserId;
        var whiteboard = CurrentWhiteboard;

        await _messagingService.CreateLine(whiteboard.WhiteboardId, line);
        
        whiteboard.AddLine(line);
        
        await SendToOthers("AddedLine", line);
    }

    public async Task AddTextShape(TextShape textShape)
    {
        if (!_whiteboardManager.IsAccepted(CurrentUserId)) return;
        
        textShape.OwnerId = CurrentUserId;
        var whiteboard = CurrentWhiteboard;
        
        await _messagingService.CreateTextShape(whiteboard.WhiteboardId, textShape);
        
        whiteboard.AddTextShape(textShape);
        
        await SendToOthers("AddedTextShape", textShape);
    }

    public async Task MoveShape(MoveShapeCommand moveShape)
    {
        if (!_whiteboardManager.IsAccepted(CurrentUserId)) return;
        
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
        if (!_whiteboardManager.IsAccepted(CurrentUserId)) return;
        
        await MoveShape(moveShape);
        
        await _messagingService.MoveShape(moveShape);
    }
}