using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Common.Authentication;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Models.User.External;
using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Application.Models.User.Command.SignUp;

public class SignUpUserCommandHandler : ICommandHandler<SignUpUserCommand, UserId>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;
    
    public SignUpUserCommandHandler(IUserRepository userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }
    
    public async Task<UserId> Handle(SignUpUserCommand command, CancellationToken cancellationToken = default)
    {
        var user = Domain.Models.User.User.Create(command.Email, command.Username); 
        
        await _authService.SignUpWithPasswordAsync(user, command.Password, cancellationToken);
        
        return user.Id;
    }
}