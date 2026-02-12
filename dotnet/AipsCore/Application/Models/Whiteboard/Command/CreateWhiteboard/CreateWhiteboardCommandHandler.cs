using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Models.Whiteboard.External;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Application.Models.Whiteboard.Command.CreateWhiteboard;

public class CreateWhiteboardCommandHandler : ICommandHandler<CreateWhiteboardCommand, WhiteboardId>
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
    
    public async Task<WhiteboardId> Handle(CreateWhiteboardCommand command, CancellationToken cancellationToken = default)
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
        
        return whiteboard.Id;
    }
}