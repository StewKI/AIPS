using AipsCore.Domain.Models.Shape.Enums;
using AipsCore.Domain.Models.Shape.Sub.Arrow;
using AipsCore.Domain.Models.Shape.Sub.Line;
using AipsCore.Domain.Models.Shape.Sub.Rectangle;
using AipsCore.Domain.Models.Shape.Sub.TextShape;

namespace AipsCore.Infrastructure.Persistence.Shape.Mappers;

public static partial class ShapeMappers
{
    public static Domain.Models.Shape.Shape MapToEntity(Shape shape)
    {
        return shape.Type switch
        {
            ShapeType.Rectangle => EntityToRectangle(shape),
            ShapeType.Line => EntityToLine(shape),
            ShapeType.Arrow => EntityToArrow(shape),
            ShapeType.Text => EntityToTextShape(shape),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static Rectangle EntityToRectangle(Shape shape)
    {
        return Rectangle.Create(
            shape.Id.ToString(),
            shape.WhiteboardId.ToString(),
            shape.AuthorId.ToString(),
            shape.PositionX, shape.PositionY,
            shape.Color,
            shape.EndPositionX!.Value,  shape.EndPositionY!.Value,
            shape.Thickness!.Value);
    }

    public static Line EntityToLine(Shape shape)
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

    public static Arrow EntityToArrow(Shape shape)
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

    public static TextShape EntityToTextShape(Shape shape)
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