using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AipsCore.Application.Abstract.UserContext;
using Microsoft.IdentityModel.Tokens;

namespace AipsCore.Infrastructure.Persistence.Authentication;

public class JwtTokenProvider : ITokenProvider
{
    private readonly JwtSettings _jwtSettings;
    
    public JwtTokenProvider(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }
    
    public string Generate(Domain.Models.User.User user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.IdValue),
            new Claim(ClaimTypes.Email, user.Email.EmailValue)
        };
        
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}