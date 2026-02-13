using AipsCore.Application.Common.Authentication;
using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.User.External;
using AipsCore.Domain.Models.User.Validation;
using AipsCore.Infrastructure.Persistence.User.Mappers;
using Microsoft.AspNetCore.Identity;

namespace AipsCore.Infrastructure.Persistence.Authentication;

public class EfAuthService : IAuthService
{
    private readonly UserManager<User.User> _userManager;
    
    public EfAuthService(UserManager<User.User> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task SignUpWithPasswordAsync(Domain.Models.User.User user, string password, CancellationToken cancellationToken = default)
    {
        var entity = user.MapToEntity();
        
        var result = await _userManager.CreateAsync(entity, password);
        
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"User registration failed: {errors}");
        }
        
        await _userManager.AddToRoleAsync(entity, UserRole.User.Name);
    }

    public async Task<LoginResult> LoginWithEmailAndPasswordAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var entity = await _userManager.FindByEmailAsync(email);

        if (entity is null)
        {
            throw new ValidationException(UserErrors.InvalidCredentials());
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(entity, password);

        if (!isPasswordValid)
        {
            throw new ValidationException(UserErrors.InvalidCredentials());
        }

        var roles = new List<UserRole>();
        var rolesNames = await _userManager.GetRolesAsync(entity);

        foreach (var roleName in rolesNames)
        {
            var role = UserRole.FromString(roleName);
            
            if (role is null) throw new Exception($"Role {roleName} not found");
            
            roles.Add(role);
        }
        
        return new LoginResult(entity.MapToModel(), roles);
    }
}