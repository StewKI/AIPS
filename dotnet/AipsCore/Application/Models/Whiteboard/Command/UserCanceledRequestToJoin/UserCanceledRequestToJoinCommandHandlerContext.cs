using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsCore.Domain.Models.WhiteboardMembership;

namespace AipsCore.Application.Models.Whiteboard.Command.UserCanceledRequestToJoin;

public record UserCanceledRequestToJoinCommandHandlerContext(
    WhiteboardId WhiteboardId,
    UserId UserId,
    WhiteboardMembership? WhiteboardMembership
    ) : ICommandHandlerContext;