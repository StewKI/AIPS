using AipsCore.Domain.Models.WhiteboardMembership.ValueObjects;

namespace AipsCore.Domain.Models.WhiteboardMembership.External;

public interface IWhiteboardMembershipRepository
{
    Task<WhiteboardMembership?> Get(WhiteboardMembershipId whiteboardMembershipId, CancellationToken cancellationToken = default);
    Task Save(WhiteboardMembership whiteboardMembership, CancellationToken cancellationToken = default);
}