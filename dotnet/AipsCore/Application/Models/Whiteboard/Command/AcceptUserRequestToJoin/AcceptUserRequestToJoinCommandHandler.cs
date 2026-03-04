using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsCore.Domain.Models.WhiteboardMembership.Enums;
using AipsCore.Domain.Models.WhiteboardMembership.External;
using AipsCore.Domain.Models.WhiteboardMembership.Validation;

namespace AipsCore.Application.Models.Whiteboard.Command.AcceptUserRequestToJoin;

public class AcceptUserRequestToJoinCommandHandler : ICommandHandler<AcceptUserRequestToJoinCommand>
{
    private readonly IWhiteboardMembershipRepository _whiteboardMembershipRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AcceptUserRequestToJoinCommandHandler(IWhiteboardMembershipRepository whiteboardMembershipRepository, IUnitOfWork unitOfWork)
    {
        _whiteboardMembershipRepository = whiteboardMembershipRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AcceptUserRequestToJoinCommand command, CancellationToken cancellationToken = default)
    {
        var whiteboardId = new WhiteboardId(command.WhiteboardId);
        var userId = new UserId(command.UserId);

        var membership = await _whiteboardMembershipRepository.GetByWhiteboardAndUserAsync(whiteboardId, userId, cancellationToken);

        if (membership is null)
        {
            throw new ValidationException(WhiteboardMembershipErrors.NotFound(whiteboardId, userId));
        }
        
        membership.UpdateStatus(WhiteboardMembershipStatus.Accepted);
        
        await _whiteboardMembershipRepository.SaveAsync(membership, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}