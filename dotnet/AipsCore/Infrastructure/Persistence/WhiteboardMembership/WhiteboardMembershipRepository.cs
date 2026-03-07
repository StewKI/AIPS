using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsCore.Domain.Models.WhiteboardMembership.External;
using AipsCore.Domain.Models.WhiteboardMembership.ValueObjects;
using AipsCore.Infrastructure.Persistence.Abstract;
using AipsCore.Infrastructure.Persistence.Db;
using Microsoft.EntityFrameworkCore;

namespace AipsCore.Infrastructure.Persistence.WhiteboardMembership;

public class WhiteboardMembershipRepository 
    : AbstractRepository<Domain.Models.WhiteboardMembership.WhiteboardMembership, WhiteboardMembershipId, WhiteboardMembership>, 
        IWhiteboardMembershipRepository
{
    public WhiteboardMembershipRepository(AipsDbContext context)
        : base(context)
    {
        
    }

    protected override Domain.Models.WhiteboardMembership.WhiteboardMembership MapToModel(WhiteboardMembership entity)
    {
        return Domain.Models.WhiteboardMembership.WhiteboardMembership.Create(
            entity.Id.ToString(),
            entity.WhiteboardId.ToString(),
            entity.UserId.ToString(),
            entity.EditingEnabled,
            entity.Status,
            entity.LastInteractedAt
        );
    }

    protected override WhiteboardMembership MapToEntity(Domain.Models.WhiteboardMembership.WhiteboardMembership model)
    {
        return new WhiteboardMembership
        {
            Id = new Guid(model.Id.IdValue),
            WhiteboardId = new Guid(model.WhiteboardId.IdValue),
            UserId = new Guid(model.UserId.IdValue),
            EditingEnabled = model.EditingEnabled.EditingEnabledValue,
            Status = model.Status,
            LastInteractedAt = model.LastInteractedAt.LastInteractedAtValue
        };
    }

    protected override void UpdateEntity(WhiteboardMembership entity, Domain.Models.WhiteboardMembership.WhiteboardMembership model)
    {
        entity.EditingEnabled = model.EditingEnabled.EditingEnabledValue;
        entity.Status = model.Status;
        entity.LastInteractedAt = model.LastInteractedAt.LastInteractedAtValue;
    }

    public async Task<Domain.Models.WhiteboardMembership.WhiteboardMembership?> GetByWhiteboardAndUserAsync(WhiteboardId whiteboardId, UserId userId, CancellationToken cancellationToken = default)
    {
        var whiteboardMembership = await Context.WhiteboardMemberships
            .FirstOrDefaultAsync((entity) =>
                    entity.WhiteboardId.ToString() == whiteboardId.IdValue &&
                    entity.UserId.ToString() == userId.IdValue,
                cancellationToken);

        if (whiteboardMembership is null) return null;
        
        return MapToModel(whiteboardMembership);
    }
}