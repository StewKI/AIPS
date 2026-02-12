using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Application.Common.Authentication;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Models.User.External;

namespace AipsCore.Application.Models.User.Command.LogIn;

public class LogInUserCommandHandler : ICommandHandler<LogInUserCommand, Token>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenProvider _tokenProvider;
    private readonly IAuthService _authService;
    
    public LogInUserCommandHandler(IUserRepository userRepository, ITokenProvider tokenProvider, IAuthService authService)
    {
        _userRepository = userRepository;
        _tokenProvider = tokenProvider;
        _authService = authService;
    }
    
    public async Task<Token> Handle(LogInUserCommand command, CancellationToken cancellationToken = default)
    {
        var loginResult = await _authService.LoginWithEmailAndPasswordAsync(command.Email, command.Password, cancellationToken);

        return new Token(_tokenProvider.Generate(loginResult.User, loginResult.Roles));
    }
}