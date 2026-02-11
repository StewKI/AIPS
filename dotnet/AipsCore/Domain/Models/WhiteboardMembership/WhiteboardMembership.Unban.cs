using AipsCore.Domain.Models.WhiteboardMembership.ValueObjects;

namespace AipsCore.Domain.Models.WhiteboardMembership;

public partial class WhiteboardMembership
{
    public void Unban()
    {
        IsBanned = new WhiteboardMembershipIsBanned(false);
    }
}