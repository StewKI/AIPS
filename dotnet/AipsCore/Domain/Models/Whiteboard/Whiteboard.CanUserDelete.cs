using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Domain.Models.Whiteboard;

public partial class Whiteboard
{
    public bool ShouldRequestToJoin(UserId userId)
    {
        return !IsOwner(userId);
    }
}