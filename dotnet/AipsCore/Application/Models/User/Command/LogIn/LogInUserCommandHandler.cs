using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Application.Authentication;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Models.User.External;

namespace AipsCore.Application.Models.User.Command.LogIn;

public class LogInUserCommandHandler : ICommandHandler<LogInUserCommand, Token>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUnitOfWork _unitOfWork;

    public LogInUserCommandHandler(IUserRepository userRepository, ITokenProvider tokenProvider, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _tokenProvider = tokenProvider;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Token> Handle(LogInUserCommand command, CancellationToken cancellationToken = default)
    {
        var loginResult = await _userRepository.LoginWithEmailAndPasswordAsync(command.Email, command.Password, cancellationToken);

        return new Token(_tokenProvider.Generate(loginResult.User, loginResult.Roles));
    }
}