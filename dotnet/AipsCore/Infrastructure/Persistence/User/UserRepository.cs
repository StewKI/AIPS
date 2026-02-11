using AipsCore.Domain.Models.User.External;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Infrastructure.Persistence.Abstract;
using AipsCore.Infrastructure.Persistence.Db;

namespace AipsCore.Infrastructure.Persistence.User;

public class UserRepository : AbstractRepository<Domain.Models.User.User, UserId, User>, IUserRepository
{
    public UserRepository(AipsDbContext context) 
        : base(context)
    {
    }

    protected override Domain.Models.User.User MapToModel(User entity)
    {
        return Domain.Models.User.User.Create(
            entity.Id.ToString(),
            entity.Email,
            entity.Username,
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
            Username = model.Username.UsernameValue,
            CreatedAt = model.CreatedAt.CreatedAtValue,
            DeletedAt = model.DeletedAt.DeletedAtValue
        };
    }

    protected override void UpdateEntity(User entity, Domain.Models.User.User model)
    {
        entity.Email = model.Email.EmailValue;
        entity.Username = model.Username.UsernameValue;
        entity.CreatedAt = model.CreatedAt.CreatedAtValue;
        entity.DeletedAt = model.DeletedAt.DeletedAtValue;
    }
}