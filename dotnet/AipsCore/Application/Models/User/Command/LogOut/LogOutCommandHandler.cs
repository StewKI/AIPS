using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.UserContext;

namespace AipsCore.Application.Models.User.Command.LogOut;

public class LogOutCommandHandler : ICommandHandler<LogOutCommand>
{
    private readonly IRefreshTokenManager _refreshTokenManager;
    
    public LogOutCommandHandler(IRefreshTokenManager refreshTokenManager)
    {
        _refreshTokenManager = refreshTokenManager;
    }
    
    public async Task Handle(LogOutCommand command, CancellationToken cancellationToken = default)
    {
        await _refreshTokenManager.RevokeAsync(command.RefreshToken, cancellationToken);
    }
}