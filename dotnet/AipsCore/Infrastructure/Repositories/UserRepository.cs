using AipsCore.Domain.Common.ValueObjects;
using AipsCore.Domain.Models.User;
using AipsCore.Domain.Models.User.External;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Infrastructure.Db;

namespace AipsCore.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AipsDbContext _context;

    public UserRepository(AipsDbContext context)
    {
        _context = context;
    }
    
    public async Task<User?> Get(UserId userId, CancellationToken cancellationToken = default)
    {
        var userEntity = await _context.Users.FindAsync([new Guid(userId.IdValue), cancellationToken], cancellationToken: cancellationToken);

        if (userEntity is null) return null;
        
        return User.Create(
            userEntity.Id.ToString(),
            userEntity.Email,
            userEntity.Username);
    }

    public async Task Save(User user, CancellationToken cancellationToken = default)
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
            userEntity = new Entities.User()
            {
                Id = new Guid(user.Id.IdValue),
                Email = user.Email.EmailValue,
                Username = user.Username.UsernameValue,
            };
            
            _context.Users.Add(userEntity);
        }
    }
}