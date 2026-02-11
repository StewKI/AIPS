using AipsCore.Domain.Models.WhiteboardMembership.External;
using AipsCore.Domain.Models.WhiteboardMembership.ValueObjects;
using AipsCore.Infrastructure.Persistence.Abstract;
using AipsCore.Infrastructure.Persistence.Db;

namespace AipsCore.Infrastructure.Persistence.WhiteboardMembership;

public class WhiteboardMembershipRepository 
    : AbstractRepository<Domain.Models.WhiteboardMembership.WhiteboardMembership, WhiteboardMembershipId, WhiteboardMembership>, 
        IWhiteboardMembershipRepository
{
    public WhiteboardMembershipRepository(AipsDbContext context)
        : base(context)
    {
        
    }

    protected override Domain.Models.WhiteboardMembership.WhiteboardMembership MapToDomainEntity(WhiteboardMembership persistenceEntity)
    {
        return Domain.Models.WhiteboardMembership.WhiteboardMembership.Create(
            persistenceEntity.Id.ToString(),
            persistenceEntity.WhiteboardId.ToString(),
            persistenceEntity.UserId.ToString(),
            persistenceEntity.IsBanned,
            persistenceEntity.EditingEnabled,
            persistenceEntity.CanJoin,
            persistenceEntity.LastInteractedAt
        );
    }

    protected override WhiteboardMembership MapToPersistenceEntity(Domain.Models.WhiteboardMembership.WhiteboardMembership domainEntity)
    {
        return new WhiteboardMembership
        {
            Id = new Guid(domainEntity.Id.IdValue),
            WhiteboardId = new Guid(domainEntity.WhiteboardId.IdValue),
            UserId = new Guid(domainEntity.UserId.IdValue),
            IsBanned = domainEntity.IsBanned.IsBannedValue,
            EditingEnabled = domainEntity.EditingEnabled.EditingEnabledValue,
            CanJoin = domainEntity.CanJoin.CanJoinValue,
            LastInteractedAt = domainEntity.LastInteractedAt.LastInteractedAtValue
        };
    }

    protected override void UpdatePersistenceEntity(WhiteboardMembership persistenceEntity, Domain.Models.WhiteboardMembership.WhiteboardMembership domainEntity)
    {
        persistenceEntity.IsBanned = domainEntity.IsBanned.IsBannedValue;
        persistenceEntity.EditingEnabled = domainEntity.EditingEnabled.EditingEnabledValue;
        persistenceEntity.CanJoin = domainEntity.CanJoin.CanJoinValue;
        persistenceEntity.LastInteractedAt = domainEntity.LastInteractedAt.LastInteractedAtValue;
    }
}