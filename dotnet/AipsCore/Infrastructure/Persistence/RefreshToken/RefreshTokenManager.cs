using AipsCore.Application.Abstract.UserContext;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Infrastructure.Authentication.Jwt;
using AipsCore.Infrastructure.Persistence.Db;
using Microsoft.EntityFrameworkCore;

namespace AipsCore.Infrastructure.Persistence.RefreshToken;

public class RefreshTokenManager : IRefreshTokenManager
{
    private readonly AipsDbContext _dbContext;
    private readonly JwtSettings _jwtSettings;

    public RefreshTokenManager(AipsDbContext dbContext, JwtSettings jwtSettings)
    {
        _dbContext = dbContext;
        _jwtSettings = jwtSettings;
    }


    public async Task AddAsync(string token, UserId userId, CancellationToken cancellationToken = default)
    {
        var refreshToken = new Persistence.RefreshToken.RefreshToken()
        {
            Id = Guid.NewGuid(),
            Token = token,
            UserId = new Guid(userId.IdValue),
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
        };
        
        await _dbContext.AddAsync(refreshToken, cancellationToken);
    }

    public async Task<Application.Common.Authentication.Models.RefreshToken> GetByValueAsync(string token, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
        
        if (entity is null)
        {
            throw RefreshTokenException.InvalidToken();
        }

        if (entity.ExpiresAt < DateTime.UtcNow)
        {
            throw RefreshTokenException.TokenExpired();
        }
        
        return entity.MapToModel();
    }

    public async Task RevokeAsync(string token, CancellationToken cancellationToken = default)
    {
        await _dbContext.RefreshTokens
            .Where(x => x.Token == token)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task RevokeAllAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        await _dbContext.RefreshTokens
            .Where(x => x.UserId == new Guid(userId.IdValue))
            .ExecuteDeleteAsync(cancellationToken);
    }
}