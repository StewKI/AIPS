using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.Shape.External;
using AipsCore.Domain.Models.Shape.Sub.Rectangle;
using AipsCore.Domain.Models.Shape.ValueObjects;
using AipsCore.Domain.Models.User.External;
using AipsCore.Domain.Models.User.Validation;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.External;
using AipsCore.Domain.Models.Whiteboard.Validation;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Application.Models.Shape.Command.CreateRectangle;

public class CreateRectangleCommandHandler : ICommandHandler<CreateRectangleCommand, ShapeId>
{
    private readonly IShapeRepository _shapeRepository;
    private readonly IWhiteboardRepository _whiteboardRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRectangleCommandHandler(IShapeRepository shapeRepository, IWhiteboardRepository whiteboardRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _shapeRepository = shapeRepository;
        _whiteboardRepository = whiteboardRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ShapeId> Handle(CreateRectangleCommand command, CancellationToken cancellationToken = default)
    {
        Validate(command);
        
        var rectangle = Rectangle.Create(
            command.WhiteboardId,
            command.AuthorId,
            command.PositionX, command.PositionY,
            command.Color,
            command.EndPositionX,
            command.EndPositionY,
            command.BorderThickness);

        await _shapeRepository.SaveAsync(rectangle, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return rectangle.Id;
    }

    private void Validate(CreateRectangleCommand command)
    {
        var whiteboardId = new WhiteboardId(command.WhiteboardId);
        var whiteboard = _whiteboardRepository.GetByIdAsync(whiteboardId).Result;

        if (whiteboard is null)
        {
            throw new ValidationException(WhiteboardErrors.NotFound(whiteboardId));
        }
        
        var authorId = new UserId(command.AuthorId);
        var author = _userRepository.GetByIdAsync(authorId).Result;

        if (author is null)
        {
            throw new ValidationException(UserErrors.NotFound(authorId));
        }
    }
}