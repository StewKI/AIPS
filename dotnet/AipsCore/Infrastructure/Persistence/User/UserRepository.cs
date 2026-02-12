using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.User.External;
using AipsCore.Domain.Models.User.Validation;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Infrastructure.Persistence.Abstract;
using AipsCore.Infrastructure.Persistence.Db;
using Microsoft.AspNetCore.Identity;

namespace AipsCore.Infrastructure.Persistence.User;

public class UserRepository : AbstractRepository<Domain.Models.User.User, UserId, User>, IUserRepository
{
    private readonly UserManager<User> _userManager;
    
    public UserRepository(AipsDbContext context, UserManager<User> userManager) 
        : base(context)
    {
        _userManager = userManager;
    }

    protected override Domain.Models.User.User MapToModel(User entity)
    {
        return Domain.Models.User.User.Create(
            entity.Id.ToString(),
            entity.Email,
            entity.UserName,
            entity.CreatedAt,
            entity.DeletedAt
        );
    }

    protected override User MapToEntity(Domain.Models.User.User model)
    {
        return new User
        {
            Id = new Guid(model.Id.IdValue),
            Email = model.Email.EmailValue,
            NormalizedEmail = model.Email.EmailValue.ToUpperInvariant(),
            UserName = model.Username.UsernameValue,
            NormalizedUserName = model.Username.UsernameValue.ToUpperInvariant(),
            CreatedAt = model.CreatedAt.CreatedAtValue,
            DeletedAt = model.DeletedAt.DeletedAtValue
        };
    }

    protected override void UpdateEntity(User entity, Domain.Models.User.User model)
    {
        entity.Email = model.Email.EmailValue;
        entity.NormalizedEmail = model.Email.EmailValue.ToUpperInvariant();
        entity.UserName = model.Username.UsernameValue;
        entity.NormalizedUserName = model.Username.UsernameValue.ToUpperInvariant();
        entity.CreatedAt = model.CreatedAt.CreatedAtValue;
        entity.DeletedAt = model.DeletedAt.DeletedAtValue;
    }

    public async Task SignUpWithPasswordAsync(Domain.Models.User.User user, string password, CancellationToken cancellationToken = default)
    {
        var entity = MapToEntity(user);
        
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
            throw new ValidationException(UserErrors.LoginErrorUserNotFoundByEmail(email));
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(entity, password);

        if (!isPasswordValid)
        {
            throw new ValidationException(UserErrors.LoginErrorIncorrectPassword());
        }

        var roles = await _userManager.GetRolesAsync(entity);
        
        return new LoginResult(MapToModel(entity), roles);
    }
}