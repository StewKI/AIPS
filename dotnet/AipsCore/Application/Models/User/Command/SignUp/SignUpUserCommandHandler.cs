using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Models.User.External;
using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Application.Models.User.Command.SignUp;

public class SignUpUserCommandHandler : ICommandHandler<SignUpUserCommand, UserId>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SignUpUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UserId> Handle(SignUpUserCommand command, CancellationToken cancellationToken = default)
    {
        var user = Domain.Models.User.User.Create(command.Email, command.Username); 
        
        await _userRepository.SignUpWithPasswordAsync(user, command.Password, cancellationToken);
        
        return user.Id;
    }
}