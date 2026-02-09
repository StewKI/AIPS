using AipsCore.Domain.Common.ValueObjects;

namespace AipsCore.Domain.Models.WhiteboardMembership.ValueObjects;

public record WhiteboardMembershipId(string IdValue) : DomainId(IdValue)
{
    public static WhiteboardMembershipId Any() => new(Guid.NewGuid().ToString());
}