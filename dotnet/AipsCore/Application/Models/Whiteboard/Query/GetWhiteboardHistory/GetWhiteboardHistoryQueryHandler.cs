using AipsCore.Application.Abstract.Query;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Domain.Models.Whiteboard.Enums;
using AipsCore.Infrastructure.Persistence.Db;
using Microsoft.EntityFrameworkCore;

namespace AipsCore.Application.Models.Whiteboard.Query.GetWhiteboardHistory;

public class GetWhiteboardHistoryQueryHandler 
    : IQueryHandler<GetWhiteboardHistoryQuery, ICollection<Infrastructure.Persistence.Whiteboard.Whiteboard>>
{
    private readonly AipsDbContext _context;
    private readonly IUserContext _userContext;

    public GetWhiteboardHistoryQueryHandler(AipsDbContext context, IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task<ICollection<Infrastructure.Persistence.Whiteboard.Whiteboard>> Handle(GetWhiteboardHistoryQuery query, CancellationToken cancellationToken = default)
    {
        var userIdGuid = new Guid(_userContext.GetCurrentUserId().IdValue);
        
        return await _context.Whiteboards
            .Where(w => w.OwnerId == userIdGuid && w.State != WhiteboardState.Deleted)
            .ToListAsync(cancellationToken);
    }
}