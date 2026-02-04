using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Domain.Models.User.External;

public interface IUserRepository
{
    Task<User?> Get(UserId userId, CancellationToken cancellationToken = default);
    Task Save(User user, CancellationToken cancellationToken = default);
}