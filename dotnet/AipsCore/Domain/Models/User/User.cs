using AipsCore.Domain.Abstract;
using AipsCore.Domain.Common.ValueObjects;
using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Domain.Models.User;

public class User : DomainModel<UserId>
{
    public Email Email { get; private set; }
    public Username Username { get; private set; }
    public CreatedAt CreatedAt { get; private set; }
    public DeletedAt DeletedAt { get; private set; }

    public User(UserId id, Email email, Username username, CreatedAt createdAt, DeletedAt deletedAt)
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
        var createdAtVo = new CreatedAt(createdAt);
        var deletedAtVo = new DeletedAt(deletedAt);
        
        return new User(userIdVo, emailVo, usernameVo, createdAtVo, deletedAtVo);
    }
    
    public static User Create(string email, string username)
    {
        var usernameVo = new Username(username);
        var emailVo = new Email(email);
        var createdAtVo = new CreatedAt(DateTime.UtcNow);
        var deletedAtVo = new DeletedAt(null);
        
        return new User(UserId.Any(), emailVo, usernameVo, createdAtVo, deletedAtVo);
    }
}