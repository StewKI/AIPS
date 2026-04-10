using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsCore.Domain.Models.WhiteboardMembership.Enums;
using AipsCore.Domain.Models.WhiteboardMembership.External;
using AipsCore.Domain.Models.WhiteboardMembership.Validation.Rules;

namespace AipsCore.Application.Models.Whiteboard.Command.AcceptUserRequestToJoin;

public sealed class AcceptUserRequestToJoinCommandHandler 
    : AbstractCommandHandler<AcceptUserRequestToJoinCommand, AcceptUserRequestToJoinCommandHandlerContext>
{
    private readonly IWhiteboardMembershipRepository _whiteboardMembershipRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AcceptUserRequestToJoinCommandHandler(IWhiteboardMembershipRepository whiteboardMembershipRepository, IUnitOfWork unitOfWork)
    {
        _whiteboardMembershipRepository = whiteboardMembershipRepository;
        _unitOfWork = unitOfWork;
    }

    protected override async Task<AcceptUserRequestToJoinCommandHandlerContext> Prepare(AcceptUserRequestToJoinCommand command, CancellationToken cancellationToken = default)
    {
        var whiteboardId = new WhiteboardId(command.WhiteboardId);
        var userId = new UserId(command.UserId);

        var membership = await _whiteboardMembershipRepository.GetByWhiteboardAndUserAsync(whiteboardId, userId, cancellationToken);

        return new AcceptUserRequestToJoinCommandHandlerContext(whiteboardId, userId, membership);
    }

    protected override async Task HandleInternal(AcceptUserRequestToJoinCommand command, AcceptUserRequestToJoinCommandHandlerContext context, CancellationToken cancellationToken = default)
    {
        var membership = context.WhiteboardMembership!;
        
        membership.UpdateStatus(WhiteboardMembershipStatus.Accepted);
        
        await _whiteboardMembershipRepository.SaveAsync(membership, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override ICollection<IRule> GetValidationRules(AcceptUserRequestToJoinCommand command, AcceptUserRequestToJoinCommandHandlerContext context)
    {
        return
        [
            new WhiteboardMembershipExistsRule(context.WhiteboardMembership, context.WhiteboardId, context.UserId),
        ];
    }
}