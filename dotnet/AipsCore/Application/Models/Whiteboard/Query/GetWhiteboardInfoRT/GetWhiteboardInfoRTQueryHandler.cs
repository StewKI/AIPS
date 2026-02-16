using AipsCore.Application.Abstract.Query;
using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.Whiteboard.Validation;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsCore.Infrastructure.Persistence.Db;
using Microsoft.EntityFrameworkCore;

namespace AipsCore.Application.Models.Whiteboard.Query.GetWhiteboardInfoRT;

public class GetWhiteboardInfoRTQueryHandler
    : IQueryHandler<GetWhiteboardInfoRTQuery, Infrastructure.Persistence.Whiteboard.Whiteboard>
{
    private readonly AipsDbContext _context;

    public GetWhiteboardInfoRTQueryHandler(AipsDbContext context)
    {
        _context = context;
    }
    
    public async Task<Infrastructure.Persistence.Whiteboard.Whiteboard> Handle(GetWhiteboardInfoRTQuery query, CancellationToken cancellationToken = default)
    {
        var whiteboard = await GetQuery(query.WhiteboardId).FirstOrDefaultAsync(cancellationToken);

        if (whiteboard is null)
        {
            throw new ValidationException(WhiteboardErrors.NotFound(new WhiteboardId(query.WhiteboardId.ToString())));
        }
        
        return whiteboard;
    }
    
    private IQueryable<Infrastructure.Persistence.Whiteboard.Whiteboard> GetQuery(Guid whiteboardId)
    {
        return _context.Whiteboards
            .Where(w => w.Id == whiteboardId)
            .Include(w => w.Memberships)
            .Include(w => w.Owner)
            .Include(w => w.Shapes);
    }
}