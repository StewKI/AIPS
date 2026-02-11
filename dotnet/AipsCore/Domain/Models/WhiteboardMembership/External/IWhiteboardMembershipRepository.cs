using AipsCore.Domain.Abstract;
using AipsCore.Domain.Models.WhiteboardMembership.ValueObjects;

namespace AipsCore.Domain.Models.WhiteboardMembership.External;

public interface IWhiteboardMembershipRepository : IAbstractRepository<WhiteboardMembership, WhiteboardMembershipId>
{
    
}