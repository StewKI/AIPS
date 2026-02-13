using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.User.External;
using AipsCore.Domain.Models.User.Validation;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Infrastructure.Persistence.Abstract;
using AipsCore.Infrastructure.Persistence.Db;
using AipsCore.Infrastructure.Persistence.User.Mappers;
using Microsoft.AspNetCore.Identity;

namespace AipsCore.Infrastructure.Persistence.User;

public class UserRepository : AbstractRepository<Domain.Models.User.User, UserId, User>, IUserRepository
{
    public UserRepository(AipsDbContext context, UserManager<User> userManager) 
        : base(context)
    {
        
    }

    protected override Domain.Models.User.User MapToModel(User entity)
    {
        return entity.MapToModel();
    }

    protected override User MapToEntity(Domain.Models.User.User model)
    {
        return model.MapToEntity();
    }

    protected override void UpdateEntity(User entity, Domain.Models.User.User model)
    {
        entity.UpdateEntity(model);
    }
}