namespace AipsCore.Infrastructure.Persistence.RefreshToken;

public static class RefreshTokenMappers
{
    public static Application.Common.Authentication.Models.RefreshToken MapToModel(this Persistence.RefreshToken.RefreshToken entity)
    {
        return new Application.Common.Authentication.Models.RefreshToken(
            entity.Token,
            entity.UserId.ToString(),
            entity.ExpiresAt);
    }
}