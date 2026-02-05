using AipsCore.Domain.Models.User.External;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Infrastructure.Persistence.Db;

namespace AipsCore.Infrastructure.Persistence.User;

public class UserRepository : IUserRepository
{
    private readonly AipsDbContext _context;

    public UserRepository(AipsDbContext context)
    {
        _context = context;
    }
    
    public async Task<Domain.Models.User.User?> Get(UserId userId, CancellationToken cancellationToken = default)
    {
        var userEntity = await _context.Users.FindAsync([new Guid(userId.IdValue), cancellationToken], cancellationToken: cancellationToken);

        if (userEntity is null) return null;
        
        return Domain.Models.User.User.Create(
            userEntity.Id.ToString(),
            userEntity.Email,
            userEntity.Username);
    }

    public async Task Save(Domain.Models.User.User user, CancellationToken cancellationToken = default)
    {
        var userEntity = await _context.Users.FindAsync([new Guid(user.Id.IdValue), cancellationToken], cancellationToken: cancellationToken);

        if (userEntity is not null)
        {
            userEntity.Email = user.Email.EmailValue;
            userEntity.Username = user.Username.UsernameValue;
            
            _context.Users.Update(userEntity);
        }
        else
        {
            userEntity = new User()
            {
                Id = new Guid(user.Id.IdValue),
                Email = user.Email.EmailValue,
                Username = user.Username.UsernameValue,
            };
            
            _context.Users.Add(userEntity);
        }
    }
}