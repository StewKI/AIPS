using AipsCore.Domain.Models.Whiteboard.Enums;
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

    protected override Domain.Models.Whiteboard.Whiteboard MapToModel(Whiteboard entity)
    {
        return Domain.Models.Whiteboard.Whiteboard.Create(
            entity.Id.ToString(),
            entity.OwnerId.ToString(),
            entity.Code,
            entity.Title,
            entity.CreatedAt,
            entity.DeletedAt,
            entity.MaxParticipants,
            entity.JoinPolicy,
            entity.State
        );
    }

    protected override Whiteboard MapToEntity(Domain.Models.Whiteboard.Whiteboard model)
    {
        return new Whiteboard
        {
            Id = new Guid(model.Id.IdValue),
            OwnerId = new Guid(model.WhiteboardOwnerId.IdValue),
            Code = model.Code.CodeValue,
            Title = model.Title.TitleValue,
            CreatedAt = model.CreatedAt.CreatedAtValue,
            DeletedAt = model.DeletedAt.DeletedAtValue,
            MaxParticipants = model.MaxParticipants.MaxParticipantsValue,
            JoinPolicy = model.JoinPolicy,
            State = model.State
        };
    }

    protected override void UpdateEntity(Whiteboard entity, Domain.Models.Whiteboard.Whiteboard model)
    {
        entity.Code = model.Code.CodeValue;
        entity.Title = model.Title.TitleValue;
        entity.CreatedAt = model.CreatedAt.CreatedAtValue;
        entity.DeletedAt = model.DeletedAt.DeletedAtValue;
        entity.MaxParticipants = model.MaxParticipants.MaxParticipantsValue;
        entity.JoinPolicy = model.JoinPolicy;
        entity.State = model.State;
    }
    
    public async Task<bool> WhiteboardCodeExists(WhiteboardCode whiteboardCode)
    {
        return await Context.Whiteboards.AnyAsync(w => w.Code == whiteboardCode.CodeValue);
    }

    public async Task SoftDeleteAsync(WhiteboardId id, CancellationToken cancellationToken = default)
    {
        var entity = await Context.Whiteboards.FindAsync([new Guid(id.IdValue)], cancellationToken);
        
        if (entity != null)
        {
            entity.State = WhiteboardState.Deleted;
            entity.DeletedAt = DateTime.UtcNow;
            Context.Whiteboards.Update(entity);
        }
    }
}