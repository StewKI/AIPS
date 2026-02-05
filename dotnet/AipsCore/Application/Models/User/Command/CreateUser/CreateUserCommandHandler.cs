using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Models.User.External;

namespace AipsCore.Application.Models.User.Command.CreateUser;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task Handle(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        var user = Domain.Models.User.User.Create(command.Email, command.Username); 
        
        await _userRepository.Save(user, cancellationToken);
    }
}