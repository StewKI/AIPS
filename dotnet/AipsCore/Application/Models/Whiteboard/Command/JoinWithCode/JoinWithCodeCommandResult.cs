using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Models.WhiteboardMembership.Enums;

namespace AipsCore.Application.Models.Whiteboard.Command.JoinWithCode;

public sealed record JoinWithCodeCommandResult(string WhiteboardId, WhiteboardMembershipStatus Status) : ICommandResult;