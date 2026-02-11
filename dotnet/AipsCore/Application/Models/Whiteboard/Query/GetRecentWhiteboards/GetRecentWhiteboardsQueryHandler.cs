using AipsCore.Application.Abstract.Query;
using AipsCore.Infrastructure.Persistence.Db;
using Microsoft.EntityFrameworkCore;

namespace AipsCore.Application.Models.Whiteboard.Query.GetRecentWhiteboards;

public class GetRecentWhiteboardsQueryHandler : IQueryHandler<GetRecentWhiteboardsQuery, ICollection<Infrastructure.Persistence.Whiteboard.Whiteboard>>
{
    private readonly AipsDbContext _context;

    public GetRecentWhiteboardsQueryHandler(AipsDbContext context)
    {
        _context = context;
    }
    
    public async Task<ICollection<Infrastructure.Persistence.Whiteboard.Whiteboard>> Handle(GetRecentWhiteboardsQuery query, CancellationToken cancellationToken = default)
    {
        return await GetQuery(query.UserId).ToListAsync(cancellationToken);
    }

    private IQueryable<Infrastructure.Persistence.Whiteboard.Whiteboard> GetQuery(string userId)
    {
        Guid userIdGuid = Guid.Parse(userId);

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