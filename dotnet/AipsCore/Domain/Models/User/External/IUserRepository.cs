using AipsCore.Domain.Abstract;
using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Domain.Models.User.External;

public interface IUserRepository : IAbstractRepository<User, UserId>
{
    Task SignUpWithPasswordAsync(User user, string password, CancellationToken cancellationToken = default);
    Task<LoginResult> LoginWithEmailAndPasswordAsync(string email, string password, CancellationToken cancellationToken = default);
}