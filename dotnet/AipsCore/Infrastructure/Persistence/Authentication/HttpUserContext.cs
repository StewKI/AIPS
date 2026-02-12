using System.Security.Claims;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Domain.Models.User.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace AipsCore.Infrastructure.Persistence.Authentication;

public class HttpUserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public HttpUserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public UserId GetCurrentUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        
        if (user is null || !user.Identity!.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }
        
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim is null)
        {
            throw new UnauthorizedAccessException("User id claim not found");
        }
        
        return new UserId(userIdClaim.Value);
    }
}