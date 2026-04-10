using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsCore.Domain.Models.WhiteboardMembership;

namespace AipsCore.Application.Models.Whiteboard.Command.RejectUserRequestToJoin;

public sealed record RejectUserRequestToJoinCommandHandlerContext(
    WhiteboardId WhiteboardId,
    UserId UserToBeRejectedId,
    UserId UserId,
    Domain.Models.Whiteboard.Whiteboard? Whiteboard,
    WhiteboardMembership? WhiteboardMembership
    ) : ICommandHandlerContext;