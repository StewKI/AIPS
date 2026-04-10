using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Application.Models.Whiteboard.Command.KickUserFromWhiteboard;

public record KickUserFromWhiteboardCommandHandlerContext(
    WhiteboardId WhiteboardId,
    UserId UserToBeKickedId,
    UserId UserId,
    Domain.Models.Whiteboard.Whiteboard? Whiteboard,
    Domain.Models.WhiteboardMembership.WhiteboardMembership? WhiteboardMembership
    ) : ICommandHandlerContext;