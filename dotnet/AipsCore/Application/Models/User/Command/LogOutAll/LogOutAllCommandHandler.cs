using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Application.Common.Command.Context;

namespace AipsCore.Application.Models.User.Command.LogOutAll;

public sealed class LogOutAllCommandHandler 
    : AbstractCommandHandler<LogOutAllCommand, EmptyCommandHandlerContext>
{
    private readonly IRefreshTokenManager _refreshTokenManager;
    private readonly IUserContext _userContext;
    
    public LogOutAllCommandHandler(IRefreshTokenManager refreshTokenManager, IUserContext userContext)
    {
        _refreshTokenManager = refreshTokenManager;
        _userContext = userContext;
    }

    protected override Task<EmptyCommandHandlerContext> Prepare(LogOutAllCommand command, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new EmptyCommandHandlerContext());
    }

    protected override Task HandleInternal(LogOutAllCommand command, EmptyCommandHandlerContext context, CancellationToken cancellationToken = default)
    {
        var userId = _userContext.GetCurrentUserId();
        
        return _refreshTokenManager.RevokeAllAsync(userId, cancellationToken);
    }
}