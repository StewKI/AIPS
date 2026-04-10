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

namespace AipsCore.Application.Models.Whiteboard.Command.BanUserFromWhiteboard;

public sealed class BanUserFromWhiteboardCommandHandler 
    : AbstractCommandHandler<BanUserFromWhiteboardCommand, BanUserFromWhiteboardCommandHandlerContext>
{
    private readonly IWhiteboardRepository _whiteboardRepository;
    private readonly IWhiteboardMembershipRepository _whiteboardMembershipRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;
    
    public BanUserFromWhiteboardCommandHandler(
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

    protected override async Task<BanUserFromWhiteboardCommandHandlerContext> Prepare(BanUserFromWhiteboardCommand command, CancellationToken cancellationToken = default)
    {
        var whiteboardId = new WhiteboardId(command.WhiteboardId);
        var userToBeBannedId = new UserId(command.UserId);
        var userId = _userContext.GetCurrentUserId();
        
        var whiteboard = await _whiteboardRepository.GetByIdAsync(whiteboardId, cancellationToken);
        
        var membership = await _whiteboardMembershipRepository.GetByWhiteboardAndUserAsync(whiteboardId, userToBeBannedId, cancellationToken);

        return new BanUserFromWhiteboardCommandHandlerContext(whiteboardId, userToBeBannedId, userId, whiteboard, membership);
    }

    protected override async Task HandleInternal(BanUserFromWhiteboardCommand command, BanUserFromWhiteboardCommandHandlerContext context, CancellationToken cancellationToken = default)
    {
        var whiteboard = context.Whiteboard!;
        var membership = context.WhiteboardMembership!;
        
        whiteboard.BanUser(membership);
        
        await _whiteboardMembershipRepository.SaveAsync(membership, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override ICollection<IRule> GetValidationRules(BanUserFromWhiteboardCommand command, BanUserFromWhiteboardCommandHandlerContext context)
    {
        return 
        [
            new DomainModelExistsRule<Domain.Models.Whiteboard.Whiteboard, WhiteboardId>(context.Whiteboard, context.WhiteboardId),
            new WhiteboardMembershipExistsRule(context.WhiteboardMembership, context.WhiteboardId, context.UserToBeBannedId),
            new OnlyOwnerCanBanOtherUsersRule(context.Whiteboard!, context.UserId),
            new UserCannotBanSelfRule(context.Whiteboard!, context.UserToBeBannedId)
        ];
    }
}