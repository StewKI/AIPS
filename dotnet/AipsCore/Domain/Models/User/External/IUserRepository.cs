using AipsCore.Domain.Abstract;
using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Domain.Models.User.External;

public interface IUserRepository : IAbstractRepository<User, UserId>
{
    
}