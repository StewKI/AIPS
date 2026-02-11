using AipsCore.Domain.Models.WhiteboardMembership.ValueObjects;

namespace AipsCore.Domain.Models.WhiteboardMembership;

public partial class WhiteboardMembership
{
    public void Kick()
    {
        CanJoin = new WhiteboardMembershipCanJoin(false);
    }
}