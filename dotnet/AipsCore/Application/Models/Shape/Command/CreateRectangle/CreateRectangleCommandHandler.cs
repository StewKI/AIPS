using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Models.Shape.External;
using AipsCore.Domain.Models.Shape.Sub.Rectangle;
using AipsCore.Domain.Models.Shape.ValueObjects;

namespace AipsCore.Application.Models.Shape.Command.CreateRectangle;

public class CreateRectangleCommandHandler : ICommandHandler<CreateRectangleCommand, ShapeId>
{
    private readonly IShapeRepository _shapeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRectangleCommandHandler(IShapeRepository shapeRepository, IUnitOfWork unitOfWork)
    {
        _shapeRepository = shapeRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ShapeId> Handle(CreateRectangleCommand command, CancellationToken cancellationToken = default)
    {
        var rectangle = Rectangle.Create(
            command.WhiteboardId,
            command.PositionX, command.PositionY,
            command.Color,
            command.EndPositionX,
            command.EndPositionY,
            command.BorderThickness);

        await _shapeRepository.Add(rectangle, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return rectangle.Id;
    }
}