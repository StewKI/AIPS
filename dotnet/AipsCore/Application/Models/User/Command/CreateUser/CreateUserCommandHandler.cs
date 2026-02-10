using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Models.User.External;
using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Application.Models.User.Command.CreateUser;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, UserId>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UserId> Handle(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        var user = Domain.Models.User.User.Create(command.Email, command.Username, command.CreatedAt, command.DeletedAt); 
        
        await _userRepository.Save(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}