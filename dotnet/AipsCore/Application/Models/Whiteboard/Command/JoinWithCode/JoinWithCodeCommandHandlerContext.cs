using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsCore.Domain.Models.WhiteboardMembership;

namespace AipsCore.Application.Models.Whiteboard.Command.JoinWithCode;

public sealed record JoinWithCodeCommandHandlerContext(
    UserId UserId,
    WhiteboardCode Code,
    Domain.Models.Whiteboard.Whiteboard? Whiteboard,
    WhiteboardMembership? WhiteboardMembership
) : ICommandHandlerContext;