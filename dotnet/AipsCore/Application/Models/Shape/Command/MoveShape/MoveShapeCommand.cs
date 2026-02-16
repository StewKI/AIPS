using AipsCore.Application.Abstract.Command;

namespace AipsCore.Application.Models.Shape.Command.MoveShape;

public record MoveShapeCommand(string ShapeId, int NewPositionX, int NewPositionY) : ICommand;