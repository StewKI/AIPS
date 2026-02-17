using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Models.Shape.ValueObjects;

namespace AipsCore.Application.Models.Shape.Command.CreateArrow;

public record CreateArrowCommand(
    string Id,
    string WhiteboardId,
    string AuthorId,
    int PositionX,
    int PositionY,
    string Color,
    int EndPositionX,
    int EndPositionY,
    int Thickness) : ICommand;