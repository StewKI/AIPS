using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Application.Abstract.UserContext;

public interface IUserContext
{
    UserId GetCurrentUserId();
}