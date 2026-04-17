using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Models.Whiteboard.External;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsCore.Domain.Models.WhiteboardMembership.External;

namespace AipsCore.Application.Models.Whiteboard.Command.JoinWithCode;

public sealed class JoinWithCodeCommandHandler 
    : AbstractStagedCommandHandler<JoinWithCodeCommand, JoinWithCodeCommandResult, JoinWithCodeCommandHandlerContext>
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
    
    protected override Task<JoinWithCodeCommandHandlerContext> InitializeContext(JoinWithCodeCommand command, CancellationToken cancellationToken = default)
    {
        
        var userId = _userContext.GetCurrentUserId();
        var code = new WhiteboardCode(command.Code);
    
        return Task.FromResult(new JoinWithCodeCommandHandlerContext(userId, code, null, null));
    }

    public override ICollection<ICommandHandlerStage<JoinWithCodeCommand, JoinWithCodeCommandResult, JoinWithCodeCommandHandlerContext>> GetCommandHandlerStages()
    {
        return
        [
            new JoinWithCodeCommandHandlerStage1(_whiteboardRepository),
            new JoinWithCodeCommandHandlerStage2(_whiteboardMembershipRepository, _unitOfWork)
        ];
    }
}