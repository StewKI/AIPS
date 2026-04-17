using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.Enums;
using AipsCore.Domain.Models.WhiteboardMembership.Enums;

namespace AipsCore.Domain.Models.Whiteboard;

public partial class Whiteboard 
{
    public WhiteboardMembership.WhiteboardMembership TryJoin(UserId userId, WhiteboardMembership.WhiteboardMembership? membership = null)
    {
        if (membership is not null)
        {
            RequestReJoin(membership);
        }
        else
        {
            membership = RequestJoin(userId);
        }
        
        return membership;
    }
    
    private WhiteboardMembership.WhiteboardMembership RequestJoin(UserId userId, WhiteboardMembership.WhiteboardMembership? membership = null)
    {
        return WhiteboardMembership.WhiteboardMembership.Create(
            Id.IdValue,
            userId.IdValue,
            false,
            DetermineJoinStatus(),
            DateTime.UtcNow);
    }

    private void RequestReJoin(WhiteboardMembership.WhiteboardMembership membership)
    {
        switch (membership.Status)
        {
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
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}  