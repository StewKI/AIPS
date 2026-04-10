using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Domain.Models.Whiteboard.External;
using AipsCore.Domain.Models.Whiteboard.Validation.Rules;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Application.Models.Whiteboard.Command.DeleteWhiteboard;

public sealed class DeleteWhiteboardCommandHandler 
    : AbstractCommandHandler<DeleteWhiteboardCommand, DeleteWhiteboardCommandHandlerContext>
{
    private readonly IUserContext _userContext;
    private readonly IWhiteboardRepository _whiteboardRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteWhiteboardCommandHandler(
        IUserContext userContext, 
        IWhiteboardRepository whiteboardRepository, 
        IUnitOfWork unitOfWork)
    {
        _userContext = userContext;
        _whiteboardRepository = whiteboardRepository;
        _unitOfWork = unitOfWork;
    }

    protected override async Task<DeleteWhiteboardCommandHandlerContext> Prepare(DeleteWhiteboardCommand command, CancellationToken cancellationToken = default)
    {
        var whiteboardId = new WhiteboardId(command.WhiteboardId);
        var userId = _userContext.GetCurrentUserId();

        var whiteboard = await _whiteboardRepository.GetByIdAsync(whiteboardId, cancellationToken);
        
        return new DeleteWhiteboardCommandHandlerContext(whiteboardId, userId, whiteboard);
    }

    protected override async Task HandleInternal(DeleteWhiteboardCommand command, DeleteWhiteboardCommandHandlerContext context, CancellationToken cancellationToken = default)
    {
        await _whiteboardRepository.SoftDeleteAsync(context.WhiteboardId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override ICollection<IRule> GetValidationRules(DeleteWhiteboardCommand command, DeleteWhiteboardCommandHandlerContext context)
    {
        return
        [
            new DomainModelExistsRule<Domain.Models.Whiteboard.Whiteboard, WhiteboardId>(context.Whiteboard, context.WhiteboardId),
            new OnlyOwnerCanDeleteWhiteboardRule(context.Whiteboard!, context.UserId)
        ];
    }
}