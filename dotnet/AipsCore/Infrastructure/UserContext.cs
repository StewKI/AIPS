using AipsCore.Application.Abstract.UserContext;
using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Infrastructure;

public class UserContext : IUserContext
{
    public UserId GetCurrentUserId()
    {
        return new UserId(new Guid("156b58b0-d0f1-4498-b2b6-afa536b68b1a").ToString());
    }
}

//Ovo je samo trenutno resenje