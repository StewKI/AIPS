using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.UserContext;

namespace AipsCore.Application.Models.User.Command.LogOutAll;

public class LogOutAllCommandHandler : ICommandHandler<LogOutAllCommand>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserContext _userContext;
    
    public LogOutAllCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUserContext userContext)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userContext = userContext;
    }
    
    public Task Handle(LogOutAllCommand command, CancellationToken cancellationToken = default)
    {
        var userId = _userContext.GetCurrentUserId();
        
        return _refreshTokenRepository.RevokeAllAsync(userId, cancellationToken);
    }
}