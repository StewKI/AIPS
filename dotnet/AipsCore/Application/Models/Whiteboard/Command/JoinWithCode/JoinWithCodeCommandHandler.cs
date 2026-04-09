using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.Whiteboard.External;
using AipsCore.Domain.Models.Whiteboard.Validation;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsCore.Domain.Models.WhiteboardMembership.Enums;
using AipsCore.Domain.Models.WhiteboardMembership.External;

namespace AipsCore.Application.Models.Whiteboard.Command.JoinWithCode;

public class JoinWithCodeCommandHandler : ICommandHandler<JoinWithCodeCommand, JoinWithCodeCommandResult>
{
    private readonly IWhiteboardRepository _whiteboardRepository;
    private readonly IWhiteboardMembershipRepository _whiteboardMembershipRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public JoinWithCodeCommandHandler(
        IWhiteboardRepository whiteboardRepository,
        IWhiteboardMembershipRepository whiteboardMembershipRepository,
        IUnitOfWork unitOfWork, 
        IUserContext userContext)
    {
        _whiteboardRepository = whiteboardRepository;
        _whiteboardMembershipRepository = whiteboardMembershipRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<JoinWithCodeCommandResult> Handle(JoinWithCodeCommand command, CancellationToken cancellationToken = default)
    {
        var userId = _userContext.GetCurrentUserId();
        
        var code = new WhiteboardCode(command.Code);
        var whiteboard = await _whiteboardRepository.GetByCodeAsync(code, cancellationToken);

        if (whiteboard is null)
        {
            throw new ValidationException(WhiteboardErrors.NotFound(code));
        }

        if (!whiteboard.ShouldRequestToJoin(userId))
        {
            return new JoinWithCodeCommandResult(whiteboard.Id.IdValue, WhiteboardMembershipStatus.Accepted);
        }

        var membership = await _whiteboardMembershipRepository.GetByWhiteboardAndUserAsync(whiteboard.Id, userId, cancellationToken);
        
        if (membership is null)
        {
            membership = whiteboard.RequestJoin(userId);
        }
        else
        {
            whiteboard.RequestReJoin(membership);
        }

        await _whiteboardMembershipRepository.SaveAsync(membership, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new JoinWithCodeCommandResult(whiteboard.Id.IdValue, membership.Status);
    }
}