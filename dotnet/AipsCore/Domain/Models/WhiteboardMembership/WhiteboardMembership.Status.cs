using AipsCore.Domain.Models.WhiteboardMembership.Enums;

namespace AipsCore.Domain.Models.WhiteboardMembership;

public partial class WhiteboardMembership
{
    public bool IsBanned()
    {
        return Status == WhiteboardMembershipStatus.Banned;
    }

    public bool IsPending()
    {
        return Status == WhiteboardMembershipStatus.Pending;
    }
    
    public void Ban()
    {
        Status = WhiteboardMembershipStatus.Banned;
    }
    
    public void Unban()
    {
        Status = WhiteboardMembershipStatus.Cancelled;
    }
    
    public void Kick()
    {
        Status = WhiteboardMembershipStatus.Kicked;
    }

    public void RejectJoinRequest()
    {
        Status = WhiteboardMembershipStatus.Rejected;
    }
    
    public void CancelJoinRequest()
    {
        Status = WhiteboardMembershipStatus.Cancelled;
    }

    public void UpdateStatus(WhiteboardMembershipStatus newStatus)
    {
        Status = newStatus;
    }
}