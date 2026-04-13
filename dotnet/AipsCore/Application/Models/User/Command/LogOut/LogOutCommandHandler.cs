using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Models.User.Validation.Rules;

namespace AipsCore.Application.Models.User.Command.LogOut;

public sealed class LogOutCommandHandler 
    : AbstractCommandHandler<LogOutCommand, LogOutCommandHandlerContext>
{
    private readonly IRefreshTokenManager _refreshTokenManager;
    private readonly IUserContext _userContext;
    
    public LogOutCommandHandler(IRefreshTokenManager refreshTokenManager, IUserContext userContext)
    {
        _refreshTokenManager = refreshTokenManager;
        _userContext = userContext;
    }

    protected override async Task<LogOutCommandHandlerContext> Prepare(LogOutCommand command, CancellationToken cancellationToken = default)
    {
        var userId = _userContext.GetCurrentUserId();
        
        var refreshToken = await _refreshTokenManager.GetByValueAsync(command.RefreshToken, cancellationToken);
        
        return new LogOutCommandHandlerContext(userId, refreshToken);
    }

    protected override async Task HandleInternal(LogOutCommand command, LogOutCommandHandlerContext context, CancellationToken cancellationToken = default)
    {
        await _refreshTokenManager.RevokeAsync(command.RefreshToken, cancellationToken);
    }

    public override ICollection<IRule> GetValidationRules(LogOutCommand command, LogOutCommandHandlerContext context)
    {
        return
        [
            new UserCanOnlyLogOutHimselfRule(context.UserId, context.RefreshToken.UserId)
        ];
    }
}