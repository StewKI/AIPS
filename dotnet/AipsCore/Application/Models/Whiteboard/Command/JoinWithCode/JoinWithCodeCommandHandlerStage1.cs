using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Models.Whiteboard.External;
using AipsCore.Domain.Models.Whiteboard.Validation.Rules;
using AipsCore.Domain.Models.WhiteboardMembership.Enums;

namespace AipsCore.Application.Models.Whiteboard.Command.JoinWithCode;

public class JoinWithCodeCommandHandlerStage1
    : AbstractCommandHandlerStage<JoinWithCodeCommand, JoinWithCodeCommandResult, JoinWithCodeCommandHandlerContext>
{
    private readonly IWhiteboardRepository _whiteboardRepository;

    public JoinWithCodeCommandHandlerStage1(IWhiteboardRepository whiteboardRepository)
    {
        _whiteboardRepository = whiteboardRepository;
    }

    protected override async Task<JoinWithCodeCommandHandlerContext> Prepare(JoinWithCodeCommand command, JoinWithCodeCommandHandlerContext context, CancellationToken cancellationToken = default)
    {
        var userId = context.UserId;
        var code = context.Code;
        
        var whiteboard = await _whiteboardRepository.GetByCodeAsync(code, cancellationToken);
        
        return new JoinWithCodeCommandHandlerContext(userId, code, whiteboard, null);
    }

    protected override Task<JoinWithCodeCommandHandlerContext> Execute(JoinWithCodeCommand command, JoinWithCodeCommandHandlerContext context, CancellationToken cancellationToken = default)
    {
        var whiteboard = context.Whiteboard!; 
        var userId = context.UserId;

        if (whiteboard.ShouldRequestToJoin(userId))
        {
            return Task.FromResult(context);
        }
        
        context.Result = new JoinWithCodeCommandResult(whiteboard.Id.IdValue, WhiteboardMembershipStatus.Accepted);

        return Task.FromResult(context);
    }

    public override ICollection<IRule> GetValidationRules(JoinWithCodeCommand command, JoinWithCodeCommandHandlerContext context)
    {
        return
        [
            new WhiteboardWithCodeExistsRule(context.Whiteboard, context.Code),
        ];
    }
}