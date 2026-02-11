using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Application.Models.Whiteboard.Command.BanUserFromWhiteboard;

public static class BanUserFromWhiteboardCommandErrors
{
    public static ValidationError WhiteboardMembershipNotFound(WhiteboardId whiteboardId, UserId userId)
        => new ValidationError(
            Code: "whiteboard_membership_not_found",
            Message: $"User with id '{userId.IdValue}' is not a member of whiteboard with id '{whiteboardId.IdValue}'");
    
    public static ValidationError WhiteboardNotFound(WhiteboardId whiteboardId)
        => new ValidationError(
            Code: "whiteboard_not_found",
            Message: $"Whiteboard with id '{whiteboardId.IdValue}' not found.");
}