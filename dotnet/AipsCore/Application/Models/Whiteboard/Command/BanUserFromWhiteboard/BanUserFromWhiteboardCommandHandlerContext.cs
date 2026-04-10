using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Application.Models.Whiteboard.Command.BanUserFromWhiteboard;

public sealed record BanUserFromWhiteboardCommandHandlerContext(
    WhiteboardId WhiteboardId,
    UserId UserToBeBannedId,
    UserId UserId,
    Domain.Models.Whiteboard.Whiteboard? Whiteboard,
    Domain.Models.WhiteboardMembership.WhiteboardMembership? WhiteboardMembership
    ) : ICommandHandlerContext;