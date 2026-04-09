using AipsCore.Application.Abstract.Query;
using AipsCore.Infrastructure.Persistence.Db;
using Microsoft.EntityFrameworkCore;

namespace AipsCore.Application.Models.Whiteboard.Query.GetWhiteboard;

public class GetWhiteboardQueryHandler : IQueryHandler<GetWhiteboardQuery, GetWhiteboardQueryResult>
{
    private readonly AipsDbContext _context;
    
    public GetWhiteboardQueryHandler(AipsDbContext context)
    {
        _context = context;
    }
    
    public async Task<GetWhiteboardQueryResult> Handle(GetWhiteboardQuery query, CancellationToken cancellationToken = default)
    {
        var whiteboard = await _context.Whiteboards
            .Where(w => w.Id.ToString() == query.WhiteboardId)
            .FirstOrDefaultAsync(cancellationToken);
        
        return new GetWhiteboardQueryResult(whiteboard);
    }
}