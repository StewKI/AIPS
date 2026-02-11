using AipsCore.Application.Abstract.UserContext;
using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Infrastructure;

public class UserContext : IUserContext
{
    public UserId GetCurrentUserId()
    {
        return new UserId(new Guid("52a1810c-802f-48b0-a74c-7b517807e392").ToString());
    }
}

//Ovo je samo trenutno resenje