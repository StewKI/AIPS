using AipsCore.Application.Abstract.Query;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Infrastructure.Persistence.Db;
using Microsoft.EntityFrameworkCore;

namespace AipsCore.Application.Models.Whiteboard.Query.GetRecentWhiteboards;

public class GetRecentWhiteboardsQueryHandler : IQueryHandler<GetRecentWhiteboardsQuery, ICollection<Infrastructure.Persistence.Whiteboard.Whiteboard>>
{
    private readonly AipsDbContext _context;
    private readonly IUserContext _userContext;

    public GetRecentWhiteboardsQueryHandler(AipsDbContext context, IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }
    
    public async Task<ICollection<Infrastructure.Persistence.Whiteboard.Whiteboard>> Handle(GetRecentWhiteboardsQuery query, CancellationToken cancellationToken = default)
    {
        var userId = _userContext.GetCurrentUserId().IdValue;
        
        return await GetQuery(userId).ToListAsync(cancellationToken);
    }

    private IQueryable<Infrastructure.Persistence.Whiteboard.Whiteboard> GetQuery(string userId)
    {
        var userIdGuid = Guid.Parse(userId);

        return _context.WhiteboardMemberships
            .Include(m => m.Whiteboard)
            .Where(m => (
                m.UserId == userIdGuid &&
                m.IsBanned == false &&
                m.Whiteboard != null
            ))
            .OrderByDescending(m => m.LastInteractedAt)
            .Select(m => m.Whiteboard!);
    }
}