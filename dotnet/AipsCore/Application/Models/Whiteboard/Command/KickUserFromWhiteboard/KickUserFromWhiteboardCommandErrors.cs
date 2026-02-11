using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Application.Models.Whiteboard.Command.KickUserFromWhiteboard;

public static class KickUserFromWhiteboardCommandErrors
{
    public static ValidationError WhiteboardMembershipNotFound(WhiteboardId whiteboardId, UserId userId)
        => new ValidationError(
            Code: "whiteboard_membership_not_found",
            Message: $"User with id '{userId}' is not a member of whiteboard with id '{whiteboardId}'");
    
    public static ValidationError WhiteboardNotFound(WhiteboardId whiteboardId)
        => new ValidationError(
            Code: "whiteboard_not_found",
            Message: $"Whiteboard with id '{whiteboardId}' not found.");
}