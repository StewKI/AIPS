using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Application.Common.Authentication;
using AipsCore.Application.Common.Authentication.Dtos;
using AipsCore.Domain.Abstract;

namespace AipsCore.Application.Models.User.Command.RefreshLogIn;

public class RefreshLogInCommandHandler : ICommandHandler<RefreshLogInCommand, LogInUserResultDto>
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    
    public RefreshLogInCommandHandler(
        ITokenProvider tokenProvider, 
        IRefreshTokenRepository refreshTokenRepository,
        IAuthService authService,
        IUnitOfWork unitOfWork)
    {
        _tokenProvider = tokenProvider;
        _refreshTokenRepository = refreshTokenRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<LogInUserResultDto> Handle(RefreshLogInCommand command, CancellationToken cancellationToken = default)
    {
        var refreshToken = await _refreshTokenRepository.GetByValueAsync(command.RefreshToken, cancellationToken);
        
        var loginResult = await _authService.LoginWithRefreshTokenAsync(refreshToken, cancellationToken);

        var newAccessToken = _tokenProvider.GenerateAccessToken(loginResult.User, loginResult.Roles);
        var newRefreshToken = _tokenProvider.GenerateRefreshToken();
        
        await _refreshTokenRepository.RevokeAsync(refreshToken.Value, cancellationToken);
        await _refreshTokenRepository.AddAsync(newRefreshToken, loginResult.User.Id, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new LogInUserResultDto(newAccessToken, newRefreshToken);
    }
}