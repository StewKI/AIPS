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

namespace AipsCore.Application.Models.Whiteboard.Command.KickUserFromWhiteboard;

public sealed class KickUserFromWhiteboardCommandHandler 
    : AbstractCommandHandler<KickUserFromWhiteboardCommand, KickUserFromWhiteboardCommandHandlerContext>
{
    private readonly IWhiteboardRepository _whiteboardRepository;
    private readonly IWhiteboardMembershipRepository _whiteboardMembershipRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;
    
    public KickUserFromWhiteboardCommandHandler(
        IWhiteboardRepository whiteboardRepository, 
        IWhiteboardMembershipRepository whiteboardMembershipRepository, 
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _whiteboardMembershipRepository = whiteboardMembershipRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
        _whiteboardRepository = whiteboardRepository;
    }

    protected override async Task<KickUserFromWhiteboardCommandHandlerContext> Prepare(KickUserFromWhiteboardCommand command, CancellationToken cancellationToken = default)
    {
        var whiteboardId = new WhiteboardId(command.WhiteboardId);
        var userToBeKickedId = new UserId(command.UserId);
        var userId = _userContext.GetCurrentUserId();

        var whiteboard = await _whiteboardRepository.GetByIdAsync(whiteboardId, cancellationToken);
        var membership = await _whiteboardMembershipRepository.GetByWhiteboardAndUserAsync(whiteboardId, userToBeKickedId, cancellationToken);

        return new KickUserFromWhiteboardCommandHandlerContext(whiteboardId, userToBeKickedId, userId, whiteboard, membership);
    }

    protected override async Task HandleInternal(KickUserFromWhiteboardCommand command, KickUserFromWhiteboardCommandHandlerContext context, CancellationToken cancellationToken = default)
    {
        var whiteboard = context.Whiteboard!;
        var membership = context.WhiteboardMembership!;
        
        whiteboard.KickUser(membership);
        
        await _whiteboardMembershipRepository.SaveAsync(membership, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override ICollection<IRule> GetValidationRules(KickUserFromWhiteboardCommand command, KickUserFromWhiteboardCommandHandlerContext context)
    {
        return 
        [
            new DomainModelExistsRule<Domain.Models.Whiteboard.Whiteboard, WhiteboardId>(context.Whiteboard, context.WhiteboardId),
            new WhiteboardMembershipExistsRule(context.WhiteboardMembership, context.WhiteboardId, context.UserToBeKickedId),
            new OnlyOwnerCanKickOtherUsersRule(context.Whiteboard!, context.UserId),
            new UserCannotKickSelfRule(context.Whiteboard!, context.UserId)
        ];
    }
}