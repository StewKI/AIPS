using AipsCore.Application.Common.Authentication.Models;
using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Application.Abstract.UserContext;

public interface IRefreshTokenRepository
{
    Task AddAsync(string token, UserId userId, CancellationToken cancellationToken = default);
    Task<RefreshToken> GetByValueAsync(string token, CancellationToken cancellationToken = default);
    Task RevokeAsync(string token, CancellationToken cancellationToken = default);
    Task RevokeAllAsync(UserId userId, CancellationToken cancellationToken = default);
}