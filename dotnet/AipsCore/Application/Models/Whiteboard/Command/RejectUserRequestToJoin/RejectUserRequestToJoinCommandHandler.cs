using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.External;
using AipsCore.Domain.Models.Whiteboard.Validation.Rules;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsCore.Domain.Models.WhiteboardMembership.External;
using AipsCore.Domain.Models.WhiteboardMembership.Validation.Rules;

namespace AipsCore.Application.Models.Whiteboard.Command.RejectUserRequestToJoin;

public sealed class RejectUserRequestToJoinCommandHandler 
    : AbstractCommandHandler<RejectUserRequestToJoinCommand, RejectUserRequestToJoinCommandHandlerContext>
{
    private readonly IWhiteboardMembershipRepository _whiteboardMembershipRepository;
    private readonly IWhiteboardRepository _whiteboardRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public RejectUserRequestToJoinCommandHandler(
        IWhiteboardMembershipRepository whiteboardMembershipRepository, 
        IWhiteboardRepository whiteboardRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _whiteboardMembershipRepository = whiteboardMembershipRepository;
        _whiteboardRepository = whiteboardRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    protected override async Task<RejectUserRequestToJoinCommandHandlerContext> Prepare(RejectUserRequestToJoinCommand command, CancellationToken cancellationToken = default)
    {
        var whiteboardId = new WhiteboardId(command.WhiteboardId);
        var userToBeRejectedId = new UserId(command.UserId);
        var userId = _userContext.GetCurrentUserId();
        
        var whiteboard = await _whiteboardRepository.GetByIdAsync(whiteboardId, cancellationToken);
        var membership = await _whiteboardMembershipRepository.GetByWhiteboardAndUserAsync(whiteboardId, userToBeRejectedId, cancellationToken);
        
        return new RejectUserRequestToJoinCommandHandlerContext(whiteboardId, userToBeRejectedId, userId, whiteboard, membership);
    }

    protected override async Task HandleInternal(RejectUserRequestToJoinCommand command, RejectUserRequestToJoinCommandHandlerContext context, CancellationToken cancellationToken = default)
    {
        var whiteboard = context.Whiteboard!;
        var membership = context.WhiteboardMembership!;
        
        whiteboard.RejectUserJoinRequest(membership);
        
        await _whiteboardMembershipRepository.SaveAsync(membership, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override ICollection<IRule> GetValidationRules(RejectUserRequestToJoinCommand command, RejectUserRequestToJoinCommandHandlerContext context)
    {
        return
        [
            new DomainModelExistsRule<Domain.Models.Whiteboard.Whiteboard, WhiteboardId>(context.Whiteboard, context.WhiteboardId),
            new WhiteboardMembershipExistsRule(context.WhiteboardMembership, context.WhiteboardId, context.UserToBeRejectedId),
            new OnlyOwnerCanRejectUsersRequestToJoinRule(context.Whiteboard!, context.UserId)
        ];
    }
}