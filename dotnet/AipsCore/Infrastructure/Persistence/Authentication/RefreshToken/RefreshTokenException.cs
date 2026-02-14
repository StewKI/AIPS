namespace AipsCore.Infrastructure.Persistence.Authentication.RefreshToken;

public class RefreshTokenException : Exception
{
    private const string InvalidTokenMessage = "Invalud token";
    private const string TokenExpiredMessage = "Token expired";
    
    public RefreshTokenException(string message)
        : base(message)
    {
        
    }
    
    public static RefreshTokenException InvalidToken() => new(InvalidTokenMessage);
    public static RefreshTokenException TokenExpired() => new(TokenExpiredMessage);
}