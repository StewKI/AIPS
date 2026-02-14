using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.UserContext;

namespace AipsCore.Application.Models.User.Command.LogOut;

public class LogOutCommandHandler : ICommandHandler<LogOutCommand>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    
    public LogOutCommandHandler(IRefreshTokenRepository refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }
    
    public async Task Handle(LogOutCommand command, CancellationToken cancellationToken = default)
    {
        await _refreshTokenRepository.RevokeAsync(command.RefreshToken, cancellationToken);
    }
}