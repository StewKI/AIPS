namespace AipsCore.Infrastructure.Persistence.User.Mappers;

public static class UserMappers
{
    public static Domain.Models.User.User MapToModel(this User entity)
    {
        return Domain.Models.User.User.Create(
            entity.Id.ToString(),
            entity.Email!,
            entity.UserName!,
            entity.CreatedAt,
            entity.DeletedAt
        );
    }
    
    public static User MapToEntity(this Domain.Models.User.User model)
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

    public static void UpdateEntity(this User entity, Domain.Models.User.User model)
    {
        entity.Email = model.Email.EmailValue;
        entity.NormalizedEmail = model.Email.EmailValue.ToUpperInvariant();
        entity.UserName = model.Username.UsernameValue;
        entity.NormalizedUserName = model.Username.UsernameValue.ToUpperInvariant();
        entity.CreatedAt = model.CreatedAt.CreatedAtValue;
        entity.DeletedAt = model.DeletedAt.DeletedAtValue;
    }
}