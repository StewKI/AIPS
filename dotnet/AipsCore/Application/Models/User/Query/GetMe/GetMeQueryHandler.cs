using AipsCore.Application.Abstract.Query;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.User.Validation;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Infrastructure.Persistence.Db;
using Microsoft.EntityFrameworkCore;

namespace AipsCore.Application.Models.User.Query.GetMe;

public class GetMeQueryHandler : IQueryHandler<GetMeQuery, GetMeQueryDto>
{
    private readonly AipsDbContext _context;
    private readonly IUserContext _userContext;

    public GetMeQueryHandler(AipsDbContext context, IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task<GetMeQueryDto> Handle(GetMeQuery query, CancellationToken cancellationToken = default)
    {
        var userId = _userContext.GetCurrentUserId();
        
        var result = await _context.Users
            .Where(u => u.Id.ToString() == userId.IdValue)
            .FirstOrDefaultAsync(cancellationToken);

        if (result is null)
        {
            throw new ValidationException(UserErrors.NotFound(new UserId(userId.IdValue)));
        }

        return new GetMeQueryDto(result.Id.ToString(), result.UserName!);
    }
}