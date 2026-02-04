using AipsCore.Domain.Common.ValueObjects;
using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Domain.Models.User;

public class User
{
    public UserId Id { get; private set; }
    public Email Email { get; private set; }
    public Username Username { get; private set; }

    public User(UserId id, Email email, Username username)
    {
        Id = id;
        Email = email;
        Username = username;
    }
    
    public static User Create(string id, string email, string username)
    {
        var userIdVo = new UserId(id);
        var usernameVo = new Username(username);
        var emailVo = new Email(email);
        return new User( userIdVo, emailVo, usernameVo);
    }
    
    public static User Create(string email, string username)
    {
        var usernameVo = new Username(username);
        var emailVo = new Email(email);
        return new User( UserId.Any(), emailVo, usernameVo);
    }
}