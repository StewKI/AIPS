using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Models.Shape.ValueObjects;

namespace AipsCore.Application.Models.Shape.Command.CreateTextShape;

public record CreateTextShapeCommand(
    string Id,
    string WhiteboardId,
    string AuthorId,
    int PositionX,
    int PositionY,
    string Color,
    string Text,
    int TextSize) : ICommand;