using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Models.Shape.External;
using AipsCore.Domain.Models.Shape.ValueObjects;

namespace AipsCore.Application.Models.Shape.Command.CreateArrow;

public class CreateArrowCommandHandler : ICommandHandler<CreateArrowCommand, ShapeId>
{
    private readonly IShapeRepository _shapeRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateArrowCommandHandler(IShapeRepository shapeRepository, IUnitOfWork unitOfWork)
    {
        _shapeRepository = shapeRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ShapeId> Handle(CreateArrowCommand command, CancellationToken cancellationToken = default)
    {
        var arrow = Domain.Models.Shape.Sub.Arrow.Arrow.Create(
            command.WhiteboardId,
            command.AuthorId,
            command.PositionX, command.PositionY,
            command.Color,
            command.EndPositionX,
            command.EndPositionY,
            command.Thickness);
        
        await _shapeRepository.SaveAsync(arrow, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return arrow.Id;
    }
}