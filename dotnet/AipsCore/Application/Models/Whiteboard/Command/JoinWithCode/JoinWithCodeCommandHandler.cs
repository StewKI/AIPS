using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Models.Whiteboard.External;
using AipsCore.Domain.Models.Whiteboard.Validation.Rules;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsCore.Domain.Models.WhiteboardMembership;
using AipsCore.Domain.Models.WhiteboardMembership.Enums;
using AipsCore.Domain.Models.WhiteboardMembership.External;

namespace AipsCore.Application.Models.Whiteboard.Command.JoinWithCode;

public sealed class JoinWithCodeCommandHandler 
    : AbstractCommandHandler<JoinWithCodeCommand, JoinWithCodeCommandResult, JoinWithCodeCommandHandlerContext>
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

    protected override async Task<JoinWithCodeCommandHandlerContext> Prepare(JoinWithCodeCommand command, CancellationToken cancellationToken = default)
    {
        var userId = _userContext.GetCurrentUserId();
        
        var code = new WhiteboardCode(command.Code);
        var whiteboard = await _whiteboardRepository.GetByCodeAsync(code, cancellationToken);
        WhiteboardMembership? membership = null;

        if (whiteboard is not null)
        {
            membership = await _whiteboardMembershipRepository.GetByWhiteboardAndUserAsync(whiteboard.Id, userId, cancellationToken);
        }
        
        return new JoinWithCodeCommandHandlerContext(userId, code, whiteboard, membership);
    }

    protected override async Task<JoinWithCodeCommandResult> HandleInternal(JoinWithCodeCommand command, JoinWithCodeCommandHandlerContext context, CancellationToken cancellationToken = default)
    {
        var whiteboard = context.Whiteboard!; 
        var userId = context.UserId;

        if (!whiteboard.ShouldRequestToJoin(userId))
        {
            return new JoinWithCodeCommandResult(whiteboard.Id.IdValue, WhiteboardMembershipStatus.Accepted);
        }

        var membership = context.WhiteboardMembership;
        
        if (membership is null)
        {
            membership = whiteboard.RequestJoin(userId);
            await _whiteboardMembershipRepository.SaveAsync(membership, cancellationToken);
        }
        else
        {
            whiteboard.RequestReJoin(membership);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new JoinWithCodeCommandResult(whiteboard.Id.IdValue, membership.Status);
    }

    public override ICollection<IRule> GetValidationRules(JoinWithCodeCommand command, JoinWithCodeCommandHandlerContext context)
    {
        return
        [
            new WhiteboardWithCodeExistsRule(context.Whiteboard, context.Code),
            new UserCannotJoinPrivateWhiteboardRule(context.Whiteboard!),
            new BannedUserCannotJoinWhiteboardRule(context.WhiteboardMembership!),
            new UserCannotJoinIfRequestIsAlreadyPendingRule(context.WhiteboardMembership!)
        ];
    }
}