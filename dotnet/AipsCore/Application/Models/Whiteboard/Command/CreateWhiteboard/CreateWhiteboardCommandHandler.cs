using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Models.Whiteboard.External;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Application.Models.Whiteboard.Command.CreateWhiteboard;

public class CreateWhiteboardCommandHandler : ICommandHandler<CreateWhiteboardCommand, WhiteboardId>
{
    private readonly IWhiteboardRepository _whiteboardRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateWhiteboardCommandHandler(IWhiteboardRepository whiteboardRepository, IUnitOfWork unitOfWork)
    {
        _whiteboardRepository = whiteboardRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<WhiteboardId> Handle(CreateWhiteboardCommand command, CancellationToken cancellationToken = default)
    {
        var whiteboardCode = await WhiteboardCode.GenerateUniqueAsync(_whiteboardRepository);
        
        var whiteboard = Domain.Models.Whiteboard.Whiteboard.Create(
            command.OwnerId,
            whiteboardCode.CodeValue,
            command.Title,
            command.MaxParticipants,
            command.JoinPolicy);

        await _whiteboardRepository.SaveAsync(whiteboard, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return whiteboard.Id;
    }
}