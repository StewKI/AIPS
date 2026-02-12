using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Models.Shape.External;
using AipsCore.Domain.Models.Shape.Sub.TextShape;
using AipsCore.Domain.Models.Shape.ValueObjects;

namespace AipsCore.Application.Models.Shape.Command.CreateTextShape;

public class CreateTextShapeCommandHandler : ICommandHandler<CreateTextShapeCommand, ShapeId>
{
    private readonly IShapeRepository _shapeRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateTextShapeCommandHandler(IShapeRepository shapeRepository, IUnitOfWork unitOfWork)
    {
        _shapeRepository = shapeRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ShapeId> Handle(CreateTextShapeCommand command, CancellationToken cancellationToken = default)
    {
        var textShape = TextShape.Create(
            command.WhiteboardId,
            command.AuthorId,
            command.PositionX, 
            command.PositionY,
            command.Color,
            command.Text,
            command.TextSize);

        await _shapeRepository.SaveAsync(textShape, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return textShape.Id;
    }
}