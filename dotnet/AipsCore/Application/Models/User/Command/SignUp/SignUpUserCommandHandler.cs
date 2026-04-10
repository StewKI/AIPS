using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Application.Common.Authentication;
using AipsCore.Application.Common.Command.Context;
using AipsCore.Domain.Abstract;

namespace AipsCore.Application.Models.User.Command.SignUp;

public sealed class SignUpUserCommandHandler 
    : AbstractCommandHandler<SignUpUserCommand, SignUpUserCommandResult, EmptyCommandHandlerContext>
{
    private readonly IAuthService _authService;
    private readonly ITokenProvider _tokenProvider;
    private readonly IRefreshTokenManager _refreshTokenManager;
    private readonly IUnitOfWork _unitOfWork;
    
    public SignUpUserCommandHandler(
        IAuthService authService, 
        ITokenProvider tokenProvider, 
        IRefreshTokenManager refreshTokenManager, 
        IUnitOfWork unitOfWork)
    {
        _authService = authService;
        _tokenProvider = tokenProvider;
        _refreshTokenManager = refreshTokenManager;
        _unitOfWork = unitOfWork;
    }

    protected override Task<EmptyCommandHandlerContext> Prepare(SignUpUserCommand command, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new EmptyCommandHandlerContext());
    }

    protected override async Task<SignUpUserCommandResult> HandleInternal(SignUpUserCommand command, EmptyCommandHandlerContext context, CancellationToken cancellationToken = default)
    {
        var user = Domain.Models.User.User.Create(command.Email, command.Username); 
        
        await _authService.SignUpWithPasswordAsync(user, command.Password, cancellationToken);
        
        var loginResult = await _authService.LoginWithEmailAndPasswordAsync(command.Email, command.Password, cancellationToken);
        
        var accessToken = _tokenProvider.GenerateAccessToken(loginResult.User, loginResult.Roles);
        var refreshToken = _tokenProvider.GenerateRefreshToken();
        
        await _refreshTokenManager.AddAsync(refreshToken, loginResult.User.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new SignUpUserCommandResult(accessToken, refreshToken);
    }
}