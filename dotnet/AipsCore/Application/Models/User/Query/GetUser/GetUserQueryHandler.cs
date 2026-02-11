using AipsCore.Application.Abstract.Query;
using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.User.Validation;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Infrastructure.Persistence.Db;
using Microsoft.EntityFrameworkCore;

namespace AipsCore.Application.Models.User.Query.GetUser;

public class GetUserQueryHandler : IQueryHandler<GetUserQuery, Infrastructure.Persistence.User.User>
{
    private readonly AipsDbContext _context;

    public GetUserQueryHandler(AipsDbContext context)
    {
        _context = context;
    }
    
    public async Task<Infrastructure.Persistence.User.User> Handle(GetUserQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _context.Users
            .Where(u => u.Id.ToString() == query.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (result is null)
        {
            throw new ValidationException(UserErrors.NotFound(new UserId(query.UserId)));
        }

        return result;
    }
}