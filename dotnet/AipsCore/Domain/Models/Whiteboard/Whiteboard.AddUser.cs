using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.Enums;
using AipsCore.Domain.Models.Whiteboard.Validation;
using AipsCore.Domain.Models.WhiteboardMembership.Enums;

namespace AipsCore.Domain.Models.Whiteboard;

public partial class Whiteboard 
{
    public WhiteboardMembership.WhiteboardMembership RequestJoin(UserId userId)
    {
        return WhiteboardMembership.WhiteboardMembership.Create(
            Id.IdValue,
            userId.IdValue,
            false,
            DetermineJoinStatus(),
            DateTime.UtcNow);
    }

    public void RequestReJoin(WhiteboardMembership.WhiteboardMembership membership)
    {
        switch (membership.Status)
        {
            case WhiteboardMembershipStatus.Banned:
                throw new ValidationException(WhiteboardErrors.UserBanned(membership.UserId));
            case WhiteboardMembershipStatus.Pending:
                throw new ValidationException(WhiteboardErrors.UserAlreadyTryingToJoin(membership.UserId));
            case WhiteboardMembershipStatus.Accepted:
                break;
            default:
                membership.UpdateStatus(DetermineJoinStatus());
                break;
        }
    }

    private WhiteboardMembershipStatus DetermineJoinStatus()
    {
        return JoinPolicy switch
        {
            WhiteboardJoinPolicy.FreeToJoin => WhiteboardMembershipStatus.Accepted,
            WhiteboardJoinPolicy.RequestToJoin => WhiteboardMembershipStatus.Pending,
            WhiteboardJoinPolicy.Private => throw new ValidationException(WhiteboardErrors.CannotJoin(Code)),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}  