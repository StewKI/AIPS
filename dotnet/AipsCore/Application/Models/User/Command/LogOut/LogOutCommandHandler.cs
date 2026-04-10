using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Application.Common.Command.Context;

namespace AipsCore.Application.Models.User.Command.LogOut;

public sealed class LogOutCommandHandler 
    : AbstractCommandHandler<LogOutCommand, EmptyCommandHandlerContext>
{
    private readonly IRefreshTokenManager _refreshTokenManager;
    
    public LogOutCommandHandler(IRefreshTokenManager refreshTokenManager)
    {
        _refreshTokenManager = refreshTokenManager;
    }

    protected override Task<EmptyCommandHandlerContext> Prepare(LogOutCommand command, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new EmptyCommandHandlerContext());
    }

    protected override async Task HandleInternal(LogOutCommand command, EmptyCommandHandlerContext context, CancellationToken cancellationToken = default)
    {
        await _refreshTokenManager.RevokeAsync(command.RefreshToken, cancellationToken);
    }
}