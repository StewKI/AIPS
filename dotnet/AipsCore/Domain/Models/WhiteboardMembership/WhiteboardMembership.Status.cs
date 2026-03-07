using AipsCore.Domain.Models.WhiteboardMembership.Enums;

namespace AipsCore.Domain.Models.WhiteboardMembership;

public partial class WhiteboardMembership
{
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

    public void UpdateStatus(WhiteboardMembershipStatus newStatus)
    {
        Status = newStatus;
    }
}