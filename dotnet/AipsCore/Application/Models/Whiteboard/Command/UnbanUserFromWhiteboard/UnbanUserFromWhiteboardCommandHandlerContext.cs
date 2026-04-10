using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Application.Models.Whiteboard.Command.UnbanUserFromWhiteboard;

public sealed record UnbanUserFromWhiteboardCommandHandlerContext(
    WhiteboardId WhiteboardId,
    UserId UserToBeUnbannedId,
    UserId UserId,
    Domain.Models.Whiteboard.Whiteboard? Whiteboard,
    Domain.Models.WhiteboardMembership.WhiteboardMembership? WhiteboardMembership
    ) : ICommandHandlerContext;