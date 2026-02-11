using AipsCore.Domain.Abstract;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsCore.Domain.Models.WhiteboardMembership.ValueObjects;

namespace AipsCore.Domain.Models.WhiteboardMembership.External;

public interface IWhiteboardMembershipRepository : IAbstractRepository<WhiteboardMembership, WhiteboardMembershipId>
{
    Task<WhiteboardMembership?> GetByWhiteboardAndUserAsync(WhiteboardId whiteboardId, UserId userId, CancellationToken cancellationToken = default);
}