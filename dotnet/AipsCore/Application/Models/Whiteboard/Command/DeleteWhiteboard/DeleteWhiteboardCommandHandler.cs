using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Abstract.Validation;
using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.Whiteboard.External;
using AipsCore.Domain.Models.Whiteboard.Validation;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Application.Models.Whiteboard.Command.DeleteWhiteboard;

public class DeleteWhiteboardCommandHandler : ICommandHandler<DeleteWhiteboardCommand>
{
    private readonly IUserContext _userContext;
    private readonly IWhiteboardRepository _whiteboardRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteWhiteboardCommandHandler(
        IUserContext userContext, 
        IWhiteboardRepository whiteboardRepository, 
        IUnitOfWork unitOfWork)
    {
        _userContext = userContext;
        _whiteboardRepository = whiteboardRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteWhiteboardCommand command, CancellationToken cancellationToken = default)
    {
        var whiteboardId = new WhiteboardId(command.WhiteboardId);
        var userId = _userContext.GetCurrentUserId();

        var whiteboard = await _whiteboardRepository.GetByIdAsync(whiteboardId, cancellationToken);

        if (whiteboard is null)
        {
            throw new ValidationException(WhiteboardErrors.NotFound(whiteboardId));
        }

        if (!whiteboard.IsOwnedBy(userId))
        {
            throw new ValidationException(WhiteboardErrors.OnlyOwnerCanDeleteWhiteboard(userId));
        }
        
        await _whiteboardRepository.SoftDeleteAsync(whiteboardId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}