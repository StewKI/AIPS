using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Application.Models.Whiteboard.Command.DeleteWhiteboard;

public sealed record DeleteWhiteboardCommandHandlerContext(
    WhiteboardId WhiteboardId,
    UserId UserId,
    Domain.Models.Whiteboard.Whiteboard? Whiteboard
    ) : ICommandHandlerContext;