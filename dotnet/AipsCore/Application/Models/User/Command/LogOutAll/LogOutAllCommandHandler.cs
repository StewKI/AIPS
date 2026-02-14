using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.UserContext;

namespace AipsCore.Application.Models.User.Command.LogOutAll;

public class LogOutAllCommandHandler : ICommandHandler<LogOutAllCommand>
{
    private readonly IRefreshTokenManager _refreshTokenManager;
    private readonly IUserContext _userContext;
    
    public LogOutAllCommandHandler(IRefreshTokenManager refreshTokenManager, IUserContext userContext)
    {
        _refreshTokenManager = refreshTokenManager;
        _userContext = userContext;
    }
    
    public Task Handle(LogOutAllCommand command, CancellationToken cancellationToken = default)
    {
        var userId = _userContext.GetCurrentUserId();
        
        return _refreshTokenManager.RevokeAllAsync(userId, cancellationToken);
    }
}