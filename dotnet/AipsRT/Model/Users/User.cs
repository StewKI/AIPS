namespace AipsRT.Model.Users;

public class User
{
    public Guid UserId { get; private set; }

    public string Username { get; private set; } 

    public string Email { get; private set; }
    
    public User(Guid userId, string username, string email)
    {
        UserId = userId;
        Username = username;
        Email = email;
    }
}