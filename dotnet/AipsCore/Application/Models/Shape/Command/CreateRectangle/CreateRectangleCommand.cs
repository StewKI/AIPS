using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Models.Shape.ValueObjects;

namespace AipsCore.Application.Models.Shape.Command.CreateRectangle;

public record CreateRectangleCommand(
    string WhiteboardId,
    int PositionX,
    int PositionY,
    string Color,
    int EndPositionX,
    int EndPositionY,
    int BorderThickness) : ICommand<ShapeId>;