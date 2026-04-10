using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Application.Common.Command.Context;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Models.Whiteboard.External;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Application.Models.Whiteboard.Command.CreateWhiteboard;

public sealed class CreateWhiteboardCommandHandler 
    : AbstractCommandHandler<CreateWhiteboardCommand, CreateWhiteboardCommandResult, EmptyCommandHandlerContext>
{
    private readonly IWhiteboardRepository _whiteboardRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public CreateWhiteboardCommandHandler(IWhiteboardRepository whiteboardRepository, IUserContext userContext, IUnitOfWork unitOfWork)
    {
        _whiteboardRepository = whiteboardRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    protected override Task<EmptyCommandHandlerContext> Prepare(CreateWhiteboardCommand command, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new EmptyCommandHandlerContext());
    }

    protected override async Task<CreateWhiteboardCommandResult> HandleInternal(CreateWhiteboardCommand command, EmptyCommandHandlerContext context, CancellationToken cancellationToken = default)
    {
        var whiteboardCode = await WhiteboardCode.GenerateUniqueAsync(_whiteboardRepository);

        var ownerId = _userContext.GetCurrentUserId();
        
        var whiteboard = Domain.Models.Whiteboard.Whiteboard.Create(
            ownerId.IdValue,
            whiteboardCode.CodeValue,
            command.Title,
            command.MaxParticipants,
            command.JoinPolicy);

        await _whiteboardRepository.SaveAsync(whiteboard, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateWhiteboardCommandResult(whiteboard.Id.IdValue);
    }
}