using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.Validation;

namespace AipsCore.Domain.Models.Whiteboard;

public partial class Whiteboard
{
    public void UnbanUser(UserId currentUserId, WhiteboardMembership.WhiteboardMembership whiteboardMembership)
    {
        if (WhiteboardOwnerId != currentUserId)
        {
            throw new ValidationException(WhiteboardErrors.OnlyOwnerCanUnbanOtherUsers(currentUserId));
        }

        whiteboardMembership.Unban();
    }
}