using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.Shape.External;
using AipsCore.Domain.Models.Shape.Validation;
using AipsCore.Domain.Models.Shape.ValueObjects;

namespace AipsCore.Application.Models.Shape.Command.MoveShape;

public class MoveShapeCommandHandler : ICommandHandler<MoveShapeCommand>
{
    private readonly IShapeRepository _shapeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MoveShapeCommandHandler(IShapeRepository shapeRepository, IUnitOfWork unitOfWork)
    {
        _shapeRepository = shapeRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(MoveShapeCommand command, CancellationToken cancellationToken = default)
    {
        var id = new ShapeId(command.ShapeId);
        var shape = await _shapeRepository.GetByIdAsync(id, cancellationToken);

        if (shape == null)
        {
            throw new ValidationException(ShapeErrors.NotFound(id));
        }
        
        shape.Move(command.NewPositionX, command.NewPositionY);
        
        await _shapeRepository.SaveAsync(shape, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}