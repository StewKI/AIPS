using AipsCore.Application.Common.Authentication.Models;
using AipsCore.Domain.Models.User;

namespace AipsCore.Application.Common.Authentication;

public interface IAuthService
{
    Task SignUpWithPasswordAsync(User user, string password, CancellationToken cancellationToken = default);
    Task<LoginResult> LoginWithEmailAndPasswordAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<LoginResult> LoginWithRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
}