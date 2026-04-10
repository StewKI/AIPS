using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Application.Common.Authentication;
using AipsCore.Application.Common.Command.Context;
using AipsCore.Domain.Abstract;

namespace AipsCore.Application.Models.User.Command.LogIn;

public sealed class LogInUserCommandHandler 
    : AbstractCommandHandler<LogInUserCommand, LogInUserCommandResult, EmptyCommandHandlerContext>
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IRefreshTokenManager _refreshTokenManager;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    
    public LogInUserCommandHandler(
        ITokenProvider tokenProvider, 
        IRefreshTokenManager refreshTokenManager, 
        IAuthService authService, 
        IUnitOfWork unitOfWork)
    {
        _tokenProvider = tokenProvider;
        _refreshTokenManager = refreshTokenManager;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    protected override Task<EmptyCommandHandlerContext> Prepare(LogInUserCommand command, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new EmptyCommandHandlerContext());
    }

    protected override async Task<LogInUserCommandResult> HandleInternal(LogInUserCommand command, EmptyCommandHandlerContext context, CancellationToken cancellationToken = default)
    {
        var loginResult = await _authService.LoginWithEmailAndPasswordAsync(command.Email, command.Password, cancellationToken);

        var accessToken = _tokenProvider.GenerateAccessToken(loginResult.User, loginResult.Roles);
        var refreshToken = _tokenProvider.GenerateRefreshToken();
        
        await _refreshTokenManager.AddAsync(refreshToken, loginResult.User.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LogInUserCommandResult(accessToken, refreshToken);
    }
}