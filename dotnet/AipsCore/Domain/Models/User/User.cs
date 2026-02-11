using AipsCore.Domain.Abstract;
using AipsCore.Domain.Common.ValueObjects;
using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Domain.Models.User;

public class User : DomainEntity<UserId>
{
    public Email Email { get; private set; }
    public Username Username { get; private set; }
    public UserCreatedAt CreatedAt { get; private set; }
    public UserDeletedAt DeletedAt { get; private set; }

    public User(UserId id, Email email, Username username, UserCreatedAt createdAt, UserDeletedAt deletedAt)
        : base(id)
    {
        Email = email;
        Username = username;
        CreatedAt = createdAt;
        DeletedAt = deletedAt;
    }
    
    public static User Create(string id, string email, string username, DateTime createdAt, DateTime? deletedAt)
    {
        var userIdVo = new UserId(id);
        var usernameVo = new Username(username);
        var emailVo = new Email(email);
        var createdAtVo = new UserCreatedAt(createdAt);
        var deletedAtVo = new UserDeletedAt(deletedAt);
        
        return new User(userIdVo, emailVo, usernameVo, createdAtVo, deletedAtVo);
    }
    
    public static User Create(string email, string username, DateTime createdAt, DateTime? deletedAt)
    {
        var usernameVo = new Username(username);
        var emailVo = new Email(email);
        var createdAtVo = new UserCreatedAt(createdAt);
        var deletedAtVo = new UserDeletedAt(deletedAt);
        
        return new User(UserId.Any(), emailVo, usernameVo, createdAtVo, deletedAtVo);
    }
}