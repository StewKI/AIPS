using AipsCore.Application.Abstract;
using AipsCore.Application.Models.User.Query.GetUser;
using AipsCore.Application.Models.Whiteboard.Command.UserCanceledRequestToJoin;

namespace AipsRT.Model.Users;

public class UserService
{
    private readonly IDispatcher _dispatcher;
    
    public UserService(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public async Task<User> GetUser(Guid userId)
    {
        var query = new GetUserQuery(userId.ToString());
        var userQueryDto = await _dispatcher.Execute(query);
        
        return new User(Guid.Parse(userQueryDto.Id), userQueryDto.UserName, userQueryDto.Email);
    }
}