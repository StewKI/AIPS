namespace AipsCore.Infrastructure.Persistence.Authentication.RefreshToken;

public static class RefreshTokenMappers
{
    public static Application.Common.Authentication.Models.RefreshToken MapToModel(this RefreshToken entity)
    {
        return new Application.Common.Authentication.Models.RefreshToken(
            entity.Token,
            entity.UserId.ToString(),
            entity.ExpiresAt);
    }
}