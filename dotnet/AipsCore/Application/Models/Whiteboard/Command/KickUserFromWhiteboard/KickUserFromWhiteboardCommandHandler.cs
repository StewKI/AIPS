using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.External;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsCore.Domain.Models.WhiteboardMembership.External;

namespace AipsCore.Application.Models.Whiteboard.Command.KickUserFromWhiteboard;

public class KickUserFromWhiteboardCommandHandler : ICommandHandler<KickUserFromWhiteboardCommand>
{
    private readonly IWhiteboardRepository _whiteboardRepository;
    private readonly IWhiteboardMembershipRepository _whiteboardMembershipRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;
    
    public KickUserFromWhiteboardCommandHandler(
        IWhiteboardRepository whiteboardRepository, 
        IWhiteboardMembershipRepository whiteboardMembershipRepository, 
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _whiteboardMembershipRepository = whiteboardMembershipRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
        _whiteboardRepository = whiteboardRepository;
    }
    
    public async Task Handle(KickUserFromWhiteboardCommand command, CancellationToken cancellationToken = default)
    {
        var whiteboardId = new WhiteboardId(command.WhiteboardId);
        var userId = new UserId(command.UserId);

        var whiteboard = await _whiteboardRepository.GetByIdAsync(whiteboardId, cancellationToken);

        if (whiteboard is null)
        {
            throw new ValidationException(KickUserFromWhiteboardCommandErrors.WhiteboardNotFound(whiteboardId));
        }
        
        var membership = await _whiteboardMembershipRepository.GetByWhiteboardAndUserAsync(whiteboardId, userId, cancellationToken);

        if (membership is null)
        {
            throw new ValidationException(KickUserFromWhiteboardCommandErrors.WhiteboardMembershipNotFound(whiteboardId, userId));
        }
        
        whiteboard.KickUser(_userContext.GetCurrentUserId(), membership);
        
        await _whiteboardMembershipRepository.SaveAsync(membership, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}