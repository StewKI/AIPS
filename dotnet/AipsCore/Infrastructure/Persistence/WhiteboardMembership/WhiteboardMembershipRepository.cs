using AipsCore.Domain.Models.WhiteboardMembership.External;
using AipsCore.Domain.Models.WhiteboardMembership.ValueObjects;
using AipsCore.Infrastructure.Persistence.Db;

namespace AipsCore.Infrastructure.Persistence.WhiteboardMembership;

public class WhiteboardMembershipRepository : IWhiteboardMembershipRepository
{
    private readonly AipsDbContext _context;
    
    public WhiteboardMembershipRepository(AipsDbContext context)
    {
        _context = context;
    }
    
    public async Task<Domain.Models.WhiteboardMembership.WhiteboardMembership?> Get(WhiteboardMembershipId whiteboardMembershipId, CancellationToken cancellationToken = default)
    {
        var whiteboardMembershipEntity = await _context.WhiteboardMemberships.FindAsync(new Guid(whiteboardMembershipId.IdValue));
        
        if (whiteboardMembershipEntity is null) return null;

        return Domain.Models.WhiteboardMembership.WhiteboardMembership.Create(
            whiteboardMembershipEntity.Id.ToString(),
            whiteboardMembershipEntity.WhiteboardId.ToString(),
            whiteboardMembershipEntity.UserId.ToString(),
            whiteboardMembershipEntity.IsBanned,
            whiteboardMembershipEntity.EditingEnabled,
            whiteboardMembershipEntity.CanJoin,
            whiteboardMembershipEntity.LastInteractedAt);
    }

    public async Task Save(Domain.Models.WhiteboardMembership.WhiteboardMembership whiteboardMembership, CancellationToken cancellationToken = default)
    {
        var whiteboardMembershipEntity = await _context.WhiteboardMemberships.FindAsync(new Guid(whiteboardMembership.Id.IdValue));
        
        if (whiteboardMembershipEntity is not null)
        {
            whiteboardMembershipEntity.IsBanned = whiteboardMembership.IsBanned.IsBannedValue;
            whiteboardMembershipEntity.EditingEnabled = whiteboardMembership.EditingEnabled.EditingEnabledValue;
            whiteboardMembershipEntity.CanJoin = whiteboardMembership.CanJoin.CanJoinValue;
            whiteboardMembershipEntity.LastInteractedAt = whiteboardMembership.LastInteractedAt.LastInteractedAtValue;
            
            _context.Update(whiteboardMembershipEntity);
        }
        else
        {
            whiteboardMembershipEntity = new WhiteboardMembership()
            {
                Id = new Guid(whiteboardMembership.Id.IdValue),
                WhiteboardId = new Guid(whiteboardMembership.WhiteboardId.IdValue),
                Whiteboard = null,
                UserId = new Guid(whiteboardMembership.UserId.IdValue),
                User = null,
                IsBanned = whiteboardMembership.IsBanned.IsBannedValue,
                EditingEnabled = whiteboardMembership.EditingEnabled.EditingEnabledValue,
                CanJoin = whiteboardMembership.CanJoin.CanJoinValue,
                LastInteractedAt = whiteboardMembership.LastInteractedAt.LastInteractedAtValue
            };
            
            _context.Add(whiteboardMembershipEntity);
        }
    }
}