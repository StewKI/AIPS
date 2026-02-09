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
        WhiteboardCode whiteboardCode;
        bool codeExists;

        do
        {
            whiteboardCode = GenerateUniqueWhiteboardCode();

            codeExists = await _whiteboardRepository.WhiteboardCodeExists(whiteboardCode);
        } while (codeExists);
        
        var whiteboard = Domain.Models.Whiteboard.Whiteboard.Create(command.OwnerId, whiteboardCode.CodeValue, command.Title);

        await _whiteboardRepository.Save(whiteboard, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return whiteboard.Id;
    }

    // TRENUTNO SAMO, CE SE NAPRAVI KO SERVIS IL KAKO VEC KASNIJE
    private static WhiteboardCode GenerateUniqueWhiteboardCode()
    {
        var rng = new Random();
        char[] result = new char[8];

        for (int i = 0; i < result.Length; i++)
        {
            result[i] = (char)('0' + rng.Next(0, 10));
        }
        
        return new WhiteboardCode(new string(result));
    }
}