using AipsCore.Domain.Models.WhiteboardMembership.Enums;

namespace AipsCore.Application.Models.Whiteboard.Command.JoinWithCode;

public record JoinWithCodeDto(string WhiteboardId, WhiteboardMembershipStatus Status);