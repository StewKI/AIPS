using AipsCore.Domain.Models.User;

namespace AipsCore.Application.Abstract.UserContext;

public interface ITokenProvider
{
    string Generate(User user, IList<string> roles);
}