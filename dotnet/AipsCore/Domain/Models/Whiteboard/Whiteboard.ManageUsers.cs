using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.Validation;

namespace AipsCore.Domain.Models.Whiteboard;

public partial class Whiteboard
{
    public void BanUser(UserId currentUserId, WhiteboardMembership.WhiteboardMembership whiteboardMembership)
    {
        if (IsOwner(currentUserId))
        {
            throw new ValidationException(WhiteboardErrors.OnlyOwnerCanBanOtherUsers(currentUserId));
        }

        whiteboardMembership.Ban();
    }
    
    public void UnbanUser(UserId currentUserId, WhiteboardMembership.WhiteboardMembership whiteboardMembership)
    {
        if (IsOwner(currentUserId))
        {
            throw new ValidationException(WhiteboardErrors.OnlyOwnerCanUnbanOtherUsers(currentUserId));
        }

        whiteboardMembership.Unban();
    }
    
    public void KickUser(UserId currentUserId, WhiteboardMembership.WhiteboardMembership whiteboardMembership)
    {
        if (IsOwner(currentUserId))
        {
            throw new ValidationException(WhiteboardErrors.OnlyOwnerCanKickOtherUsers(currentUserId));
        }

        whiteboardMembership.Kick();
    }

    private bool IsOwner(UserId userId)
    {
        return WhiteboardOwnerId.IdValue == userId.IdValue;
    }
}