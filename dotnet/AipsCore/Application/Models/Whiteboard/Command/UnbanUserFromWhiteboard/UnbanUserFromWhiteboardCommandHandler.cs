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

namespace AipsCore.Application.Models.Whiteboard.Command.UnbanUserFromWhiteboard;

public sealed class UnbanUserFromWhiteboardCommandHandler 
    : AbstractCommandHandler<UnbanUserFromWhiteboardCommand, UnbanUserFromWhiteboardCommandHandlerContext>
{
    private readonly IWhiteboardRepository _whiteboardRepository;
    private readonly IWhiteboardMembershipRepository _whiteboardMembershipRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public UnbanUserFromWhiteboardCommandHandler(
        IWhiteboardRepository whiteboardRepository, 
        IWhiteboardMembershipRepository whiteboardMembershipRepository, 
        IUserContext userContext, 
        IUnitOfWork unitOfWork)
    {
        _whiteboardRepository = whiteboardRepository;
        _whiteboardMembershipRepository = whiteboardMembershipRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    protected override async Task<UnbanUserFromWhiteboardCommandHandlerContext> Prepare(UnbanUserFromWhiteboardCommand command, CancellationToken cancellationToken = default)
    {
        var whiteboardId = new WhiteboardId(command.WhiteboardId);
        var userToBeUnbannedId = new UserId(command.UserId);
        var userId = _userContext.GetCurrentUserId();
        
        var whiteboard = await _whiteboardRepository.GetByIdAsync(whiteboardId, cancellationToken);
        var membership = await _whiteboardMembershipRepository.GetByWhiteboardAndUserAsync(whiteboardId, userToBeUnbannedId, cancellationToken);

        return new UnbanUserFromWhiteboardCommandHandlerContext(whiteboardId, userToBeUnbannedId, userId, whiteboard, membership);
    }

    protected override async Task HandleInternal(UnbanUserFromWhiteboardCommand command, UnbanUserFromWhiteboardCommandHandlerContext context, CancellationToken cancellationToken = default)
    {
        var whiteboard = context.Whiteboard!;
        var membership = context.WhiteboardMembership!;
        
        whiteboard.UnbanUser(membership);
        
        await _whiteboardMembershipRepository.SaveAsync(membership, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override ICollection<IRule> GetValidationRules(UnbanUserFromWhiteboardCommand command, UnbanUserFromWhiteboardCommandHandlerContext context)
    {
        return
        [
            new DomainModelExistsRule<Domain.Models.Whiteboard.Whiteboard, WhiteboardId>(context.Whiteboard, context.WhiteboardId),
            new WhiteboardMembershipExistsRule(context.WhiteboardMembership, context.WhiteboardId, context.UserToBeUnbannedId),
            new OnlyOwnerCanUnbanOtherUsersRule(context.Whiteboard!, context.UserId)
        ];
    }
}