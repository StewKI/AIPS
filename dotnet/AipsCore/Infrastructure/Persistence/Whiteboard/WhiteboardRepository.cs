using AipsCore.Domain.Models.Whiteboard.External;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsCore.Infrastructure.Persistence.Abstract;
using AipsCore.Infrastructure.Persistence.Db;
using Microsoft.EntityFrameworkCore;

namespace AipsCore.Infrastructure.Persistence.Whiteboard;

public class WhiteboardRepository 
    : AbstractRepository<Domain.Models.Whiteboard.Whiteboard, WhiteboardId, Whiteboard>, IWhiteboardRepository
{
    public WhiteboardRepository(AipsDbContext context) 
        : base(context)
    {
        
    }

    protected override Domain.Models.Whiteboard.Whiteboard MapToDomainEntity(Whiteboard persistenceEntity)
    {
        return Domain.Models.Whiteboard.Whiteboard.Create(
            persistenceEntity.Id.ToString(),
            persistenceEntity.OwnerId.ToString(),
            persistenceEntity.Code,
            persistenceEntity.Title,
            persistenceEntity.CreatedAt,
            persistenceEntity.DeletedAt,
            persistenceEntity.MaxParticipants,
            persistenceEntity.JoinPolicy,
            persistenceEntity.State
        );
    }

    protected override Whiteboard MapToPersistenceEntity(Domain.Models.Whiteboard.Whiteboard domainEntity)
    {
        return new Whiteboard
        {
            Id = new Guid(domainEntity.Id.IdValue),
            OwnerId = new Guid(domainEntity.WhiteboardOwnerId.IdValue),
            Code = domainEntity.Code.CodeValue,
            Title = domainEntity.Title.TitleValue,
            CreatedAt = domainEntity.CreatedAt.CreatedAtValue,
            DeletedAt = domainEntity.DeletedAt.DeletedAtValue,
            MaxParticipants = domainEntity.MaxParticipants.MaxParticipantsValue,
            JoinPolicy = domainEntity.JoinPolicy,
            State = domainEntity.State
        };
    }

    protected override void UpdatePersistenceEntity(Whiteboard persistenceEntity, Domain.Models.Whiteboard.Whiteboard domainEntity)
    {
        persistenceEntity.Code = domainEntity.Code.CodeValue;
        persistenceEntity.Title = domainEntity.Title.TitleValue;
        persistenceEntity.CreatedAt = domainEntity.CreatedAt.CreatedAtValue;
        persistenceEntity.DeletedAt = domainEntity.DeletedAt.DeletedAtValue;
        persistenceEntity.MaxParticipants = domainEntity.MaxParticipants.MaxParticipantsValue;
        persistenceEntity.JoinPolicy = domainEntity.JoinPolicy;
        persistenceEntity.State = domainEntity.State;
    }
    
    public async Task<bool> WhiteboardCodeExists(WhiteboardCode whiteboardCode)
    {
        return await Context.Whiteboards.AnyAsync(w => w.Code == whiteboardCode.CodeValue);
    }
}