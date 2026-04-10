using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Domain.Models.Whiteboard;

public partial class Whiteboard
{
    public void BanUser(WhiteboardMembership.WhiteboardMembership whiteboardMembership)
    {
        whiteboardMembership.Ban();
    }
    
    public void UnbanUser(WhiteboardMembership.WhiteboardMembership whiteboardMembership)
    {
        whiteboardMembership.Unban();
    }
    
    public void KickUser(WhiteboardMembership.WhiteboardMembership whiteboardMembership)
    {
        whiteboardMembership.Kick();
    }
    
    public void RejectUserJoinRequest(WhiteboardMembership.WhiteboardMembership whiteboardMembership)
    {
        whiteboardMembership.RejectJoinRequest();
    }

    private bool IsOwner(UserId userId)
    {
        return WhiteboardOwnerId.IdValue == userId.IdValue;
    }
}