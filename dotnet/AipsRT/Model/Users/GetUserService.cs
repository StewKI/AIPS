using AipsCore.Application.Abstract;
using AipsCore.Application.Models.User.Query.GetUser;

namespace AipsRT.Model.Users;

public class GetUserService
{
    private readonly IDispatcher _dispatcher;
    
    public GetUserService(IDispatcher dispatcher)
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