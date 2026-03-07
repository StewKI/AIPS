using AipsCore.Domain.Abstract.Validation;
using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsCore.Domain.Models.WhiteboardMembership.ValueObjects;

namespace AipsCore.Domain.Models.WhiteboardMembership.Validation;

public class WhiteboardMembershipErrors : AbstractErrors<WhiteboardMembership, WhiteboardMembershipId>
{
    public static ValidationError NotFound(WhiteboardId whiteboardId, UserId userId)
    {
        const string code = "whiteboard_membership_not_found";
        string message = $"Whiteboard membership with whiteboard id {whiteboardId.IdValue} and user id {userId.IdValue} not found.";
        
        return CreateValidationError(code, message);
    }
}