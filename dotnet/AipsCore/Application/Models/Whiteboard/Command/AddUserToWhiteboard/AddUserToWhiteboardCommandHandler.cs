using AipsCore.Application.Abstract;
using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.User.External;
using AipsCore.Domain.Models.User.Validation;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.External;
using AipsCore.Domain.Models.Whiteboard.Validation;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsCore.Domain.Models.WhiteboardMembership.External;

namespace AipsCore.Application.Models.Whiteboard.Command.AddUserToWhiteboard;

public class AddUserToWhiteboardCommandHandler
    : ICommandHandler<AddUserToWhiteboardCommand>
{
    private readonly IWhiteboardRepository _whiteboardRepository;
    private readonly IWhiteboardMembershipRepository _whiteboardMembershipRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDispatcher _dispatcher;

    public AddUserToWhiteboardCommandHandler(
        IWhiteboardRepository whiteboardRepository,
        IWhiteboardMembershipRepository whiteboardMembershipRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IDispatcher dispatcher)
    {
        _whiteboardRepository = whiteboardRepository;
        _whiteboardMembershipRepository = whiteboardMembershipRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _dispatcher = dispatcher;
    }

    private Domain.Models.Whiteboard.Whiteboard? _whiteboard;
    private Domain.Models.User.User? _user;
    
    public async Task Handle(AddUserToWhiteboardCommand command, CancellationToken cancellationToken = default)
    {
        _whiteboard = await _whiteboardRepository.GetByIdAsync(new WhiteboardId(command.WhiteboardId), cancellationToken);
        _user = await _userRepository.GetByIdAsync(new UserId(command.UserId), cancellationToken);
        
        Validate(command);
        
        await _whiteboard!.AddUserAsync(_user!, _whiteboardMembershipRepository, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private void Validate(AddUserToWhiteboardCommand command)
    {
        if (_whiteboard is null)
        {
            throw new ValidationException(WhiteboardErrors.NotFound(new WhiteboardId(command.WhiteboardId)));
        }

        if (_user is null)
        {
            throw new ValidationException(UserErrors.NotFound(new UserId(command.UserId)));
        }
    }
}