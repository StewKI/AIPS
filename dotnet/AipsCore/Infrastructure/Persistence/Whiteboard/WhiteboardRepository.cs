using AipsCore.Domain.Models.Whiteboard.External;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsCore.Infrastructure.Persistence.Db;
using Microsoft.EntityFrameworkCore;

namespace AipsCore.Infrastructure.Persistence.Whiteboard;

public class WhiteboardRepository : IWhiteboardRepository
{
    private readonly AipsDbContext _context;
    
    public WhiteboardRepository(AipsDbContext context)
    {
        _context = context;
    }
    
    public async Task<Domain.Models.Whiteboard.Whiteboard?> Get(WhiteboardId whiteboardId, CancellationToken cancellationToken = default)
    {
        var whiteboardEntity = await _context.Whiteboards.FindAsync([new Guid(whiteboardId.IdValue), cancellationToken], cancellationToken: cancellationToken);

        if (whiteboardEntity is null) return null;

        return Domain.Models.Whiteboard.Whiteboard.Create(
            whiteboardEntity.Id.ToString(),
            whiteboardEntity.OwnerId.ToString(),
            whiteboardEntity.Code,
            whiteboardEntity.Title,
            whiteboardEntity.CreatedAt,
            whiteboardEntity.DeletedAt,
            whiteboardEntity.MaxParticipants,
            whiteboardEntity.JoinPolicy,
            whiteboardEntity.State);
    }

    public async Task Save(Domain.Models.Whiteboard.Whiteboard whiteboard, CancellationToken cancellationToken = default)
    {
        var whiteboardEntity = await _context.Whiteboards.FindAsync(new Guid(whiteboard.Id.IdValue));

        if (whiteboardEntity is not null)
        {
            whiteboardEntity.OwnerId = new Guid(whiteboard.WhiteboardOwnerId.IdValue);
            whiteboardEntity.Code = whiteboard.Code.CodeValue;
            whiteboardEntity.Title = whiteboard.Title.TitleValue;
            whiteboardEntity.CreatedAt = whiteboard.CreatedAt.CreatedAtValue;
            whiteboardEntity.DeletedAt = whiteboard.DeletedAt.DeletedAtValue;
            whiteboardEntity.MaxParticipants = whiteboard.MaxParticipants.MaxParticipantsValue;
            whiteboardEntity.JoinPolicy = whiteboard.JoinPolicy;
            whiteboardEntity.State = whiteboard.State;
            
            _context.Whiteboards.Update(whiteboardEntity);
        }
        else
        {
            whiteboardEntity = new Whiteboard()
            {
                Id = new Guid(whiteboard.Id.IdValue),
                OwnerId = new Guid(whiteboard.WhiteboardOwnerId.IdValue),
                Code = whiteboard.Code.CodeValue,
                Title = whiteboard.Title.TitleValue,
                CreatedAt = whiteboard.CreatedAt.CreatedAtValue,
                DeletedAt = whiteboard.DeletedAt.DeletedAtValue,
                MaxParticipants = whiteboard.MaxParticipants.MaxParticipantsValue,
                JoinPolicy = whiteboard.JoinPolicy,
                State = whiteboard.State
            };
            
            _context.Whiteboards.Add(whiteboardEntity);
        }
    }

    public async Task<bool> WhiteboardCodeExists(WhiteboardCode whiteboardCode)
    {
        var codeExists = await _context.Whiteboards.AnyAsync(w => w.Code == whiteboardCode.CodeValue);
        return codeExists;
    }
}