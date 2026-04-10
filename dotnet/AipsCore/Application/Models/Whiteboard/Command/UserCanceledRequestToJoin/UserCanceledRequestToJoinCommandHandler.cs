using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsCore.Domain.Models.WhiteboardMembership.Enums;
using AipsCore.Domain.Models.WhiteboardMembership.External;
using AipsCore.Domain.Models.WhiteboardMembership.Validation;
using AipsCore.Domain.Models.WhiteboardMembership.Validation.Rules;

namespace AipsCore.Application.Models.Whiteboard.Command.UserCanceledRequestToJoin;

public class CancelJoinRequestCommandHandler 
    : AbstractCommandHandler<UserCanceledRequestToJoinCommand, UserCanceledRequestToJoinCommandHandlerContext>
{
    private readonly IWhiteboardMembershipRepository _whiteboardMembershipRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelJoinRequestCommandHandler(IWhiteboardMembershipRepository whiteboardMembershipRepository, IUnitOfWork unitOfWork)
    {
        _whiteboardMembershipRepository = whiteboardMembershipRepository;
        _unitOfWork = unitOfWork;
    }

    protected override async Task<UserCanceledRequestToJoinCommandHandlerContext> Prepare(UserCanceledRequestToJoinCommand command, CancellationToken cancellationToken = default)
    {
        var whiteboardId = new WhiteboardId(command.WhiteboardId);
        var userId = new UserId(command.UserId);
        
        var membership = await _whiteboardMembershipRepository.GetByWhiteboardAndUserAsync(whiteboardId, userId, cancellationToken);

        return new UserCanceledRequestToJoinCommandHandlerContext(whiteboardId, userId, membership);
    }

    protected override async Task HandleInternal(UserCanceledRequestToJoinCommand command, UserCanceledRequestToJoinCommandHandlerContext context, CancellationToken cancellationToken = default)
    {
        var membership = context.WhiteboardMembership!;
        
        membership.CancelJoinRequest();
        
        await _whiteboardMembershipRepository.SaveAsync(membership, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override ICollection<IRule> GetValidationRules(UserCanceledRequestToJoinCommand command, UserCanceledRequestToJoinCommandHandlerContext context)
    {
        return
        [
            new WhiteboardMembershipExistsRule(context.WhiteboardMembership, context.WhiteboardId, context.UserId),
        ];
    }
}