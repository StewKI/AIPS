using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Models.Whiteboard.Validation.Rules;
using AipsCore.Domain.Models.WhiteboardMembership.External;

namespace AipsCore.Application.Models.Whiteboard.Command.JoinWithCode;

public class JoinWithCodeCommandHandlerStage2
    : AbstractCommandHandlerStage<JoinWithCodeCommand, JoinWithCodeCommandResult, JoinWithCodeCommandHandlerContext>
{
    private readonly IWhiteboardMembershipRepository _whiteboardMembershipRepository;
    private readonly IUnitOfWork _unitOfWork;

    public JoinWithCodeCommandHandlerStage2(IWhiteboardMembershipRepository whiteboardMembershipRepository, IUnitOfWork unitOfWork)
    {
        _whiteboardMembershipRepository = whiteboardMembershipRepository;
        _unitOfWork = unitOfWork;
    }

    protected override Task<JoinWithCodeCommandHandlerContext> Prepare(JoinWithCodeCommand command, JoinWithCodeCommandHandlerContext context, CancellationToken cancellationToken = default)
    {
        var userId = context.UserId;
        var whiteboard = context.Whiteboard!;
        var membership = context.WhiteboardMembership;

        membership = whiteboard.TryJoin(userId, membership);
        
        return Task.FromResult(new JoinWithCodeCommandHandlerContext(userId, context.Code, whiteboard, membership));
    }

    protected override async Task<JoinWithCodeCommandHandlerContext> Execute(JoinWithCodeCommand command, JoinWithCodeCommandHandlerContext context, CancellationToken cancellationToken = default)
    {
        var whiteboard = context.Whiteboard!;
        var membership = context.WhiteboardMembership!;
        
        await _whiteboardMembershipRepository.SaveAsync(membership, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        context.Result = new JoinWithCodeCommandResult(whiteboard.Id.IdValue, membership.Status);
        return context;
    }
    

    public override ICollection<IRule> GetValidationRules(JoinWithCodeCommand command, JoinWithCodeCommandHandlerContext context)
    {
        return
        [
            new UserCannotJoinPrivateWhiteboardRule(context.Whiteboard!),
            new BannedUserCannotJoinWhiteboardRule(context.WhiteboardMembership!),
            new UserCannotJoinIfRequestIsAlreadyPendingRule(context.WhiteboardMembership!)
        ];
    }
}