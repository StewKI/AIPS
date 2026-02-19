using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Domain.Models.Whiteboard;

public partial class Whiteboard
{
    public bool CanUserDelete(UserId userId)
    {
        return WhiteboardOwnerId.IdValue == userId.IdValue;
    }
}