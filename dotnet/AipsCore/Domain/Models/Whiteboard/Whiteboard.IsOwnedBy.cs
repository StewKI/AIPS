using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Domain.Models.Whiteboard;

public partial class Whiteboard
{
    public bool IsOwnedBy(UserId userId)
    {
        return WhiteboardOwnerId.IdValue == userId.IdValue;
    }
}