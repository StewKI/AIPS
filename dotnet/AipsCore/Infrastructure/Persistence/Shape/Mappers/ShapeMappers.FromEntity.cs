using AipsCore.Domain.Models.Shape.Enums;
using AipsCore.Domain.Models.Shape.Sub.Arrow;
using AipsCore.Domain.Models.Shape.Sub.Line;
using AipsCore.Domain.Models.Shape.Sub.Rectangle;
using AipsCore.Domain.Models.Shape.Sub.TextShape;

namespace AipsCore.Infrastructure.Persistence.Shape.Mappers;

public static partial class ShapeMappers
{
    public static Domain.Models.Shape.Shape MapToDomainEntity(Shape shape)
    {
        return shape.Type switch
        {
            ShapeType.Rectangle => PersistenceEntityToRectangle(shape),
            ShapeType.Line => PersistenceEntityToLine(shape),
            ShapeType.Arrow => PersistenceEntityToArrow(shape),
            ShapeType.Text => PersistenceEntityToTextShape(shape),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static Rectangle PersistenceEntityToRectangle(Shape shape)
    {
        return Rectangle.Create(
            shape.Id.ToString(),
            shape.WhiteboardId.ToString(),
            shape.PositionX, shape.PositionY,
            shape.Color,
            shape.EndPositionX!.Value,  shape.EndPositionY!.Value,
            shape.Thickness!.Value);
    }

    private static Line PersistenceEntityToLine(Shape shape)
    {
        return Line.Create(
            shape.Id.ToString(),
            shape.WhiteboardId.ToString(),
            shape.AuthorId.ToString(),
            shape.PositionX, shape.PositionY,
            shape.Color,
            shape.EndPositionX!.Value,  shape.EndPositionY!.Value,
            shape.Thickness!.Value);
    }

    private static Arrow PersistenceEntityToArrow(Shape shape)
    {
        return Arrow.Create(
            shape.Id.ToString(),
            shape.WhiteboardId.ToString(),
            shape.AuthorId.ToString(),
            shape.PositionX, shape.PositionY,
            shape.Color,
            shape.EndPositionX!.Value,  shape.EndPositionY!.Value,
            shape.Thickness!.Value);
    }

    private static TextShape PersistenceEntityToTextShape(Shape shape)
    {
        return TextShape.Create(
            shape.Id.ToString(),
            shape.WhiteboardId.ToString(),
            shape.AuthorId.ToString(),
            shape.PositionX, shape.PositionY,
            shape.Color,
            shape.TextValue!, shape.TextSize!.Value);
    }
}