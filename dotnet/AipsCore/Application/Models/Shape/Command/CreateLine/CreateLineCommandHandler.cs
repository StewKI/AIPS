using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Models.Shape.External;
using AipsCore.Domain.Models.Shape.ValueObjects;

namespace AipsCore.Application.Models.Shape.Command.CreateLine;

public class CreateLineCommandHandler : ICommandHandler<CreateLineCommand>
{
    private readonly IShapeRepository _shapeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLineCommandHandler(IShapeRepository shapeRepository, IUnitOfWork unitOfWork)
    {
        _shapeRepository = shapeRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(CreateLineCommand command, CancellationToken cancellationToken = default)
    {
        var line = Domain.Models.Shape.Sub.Line.Line.Create(
            command.Id,
            command.WhiteboardId,
            command.AuthorId,
            command.PositionX, 
            command.PositionY,
            command.Color,
            command.EndPositionX,
            command.EndPositionY,
            command.Thickness);

        await _shapeRepository.SaveAsync(line, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}