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

    protected override Domain.Models.User.User MapToDomainEntity(User persistenceEntity)
    {
        return Domain.Models.User.User.Create(
            persistenceEntity.Id.ToString(),
            persistenceEntity.Email,
            persistenceEntity.Username,
            persistenceEntity.CreatedAt,
            persistenceEntity.DeletedAt
        );
    }

    protected override User MapToPersistenceEntity(Domain.Models.User.User domainEntity)
    {
        return new User
        {
            Id = new Guid(domainEntity.Id.IdValue),
            Email = domainEntity.Email.EmailValue,
            Username = domainEntity.Username.UsernameValue,
            CreatedAt = domainEntity.CreatedAt.CreatedAtValue,
            DeletedAt = domainEntity.DeletedAt.DeletedAtValue
        };
    }

    protected override void UpdatePersistenceEntity(User persistenceEntity, Domain.Models.User.User domainEntity)
    {
        persistenceEntity.Email = domainEntity.Email.EmailValue;
        persistenceEntity.Username = domainEntity.Username.UsernameValue;
        persistenceEntity.CreatedAt = domainEntity.CreatedAt.CreatedAtValue;
        persistenceEntity.DeletedAt = domainEntity.DeletedAt.DeletedAtValue;
    }
}