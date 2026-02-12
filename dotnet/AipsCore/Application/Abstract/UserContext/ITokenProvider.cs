using AipsCore.Domain.Models.User;
using AipsCore.Domain.Models.User.External;

namespace AipsCore.Application.Abstract.UserContext;

public interface ITokenProvider
{
    string Generate(User user, IList<UserRole> roles);
}