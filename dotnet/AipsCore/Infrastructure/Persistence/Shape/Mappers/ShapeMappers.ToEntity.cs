using AipsCore.Domain.Models.Shape.Enums;
using AipsCore.Domain.Models.Shape.Sub.Arrow;
using AipsCore.Domain.Models.Shape.Sub.Line;
using AipsCore.Domain.Models.Shape.Sub.Rectangle;
using AipsCore.Domain.Models.Shape.Sub.TextShape;

namespace AipsCore.Infrastructure.Persistence.Shape.Mappers;

public static partial class ShapeMappers
{
    
    public static Shape MapToEntity(Domain.Models.Shape.Shape model)
    {
        return model.ShapeType switch
        {
            ShapeType.Rectangle => RectangleToEntity((Rectangle)model),
            ShapeType.Line => LineToEntity((Line)model),
            ShapeType.Arrow => ArrowToEntity((Arrow)model),
            ShapeType.Text => TextShapeToEntity((TextShape)model),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static Shape RectangleToEntity(Rectangle rectangle)
    {
        return new Shape
        {
            Id = new Guid(rectangle.Id.Value),
            Type = rectangle.ShapeType,
            WhiteboardId = new Guid(rectangle.WhiteboardId.IdValue),
            AuthorId = new Guid(rectangle.AuthorId.IdValue),
            PositionX = rectangle.Position.X,
            PositionY = rectangle.Position.Y,
            Color = rectangle.Color.Value,
            //SPECIFIC
            EndPositionX = rectangle.EndPosition.X,
            EndPositionY = rectangle.EndPosition.Y,
            Thickness = rectangle.BorderThickness.Value,
        };
    }

    public static Shape LineToEntity(Line line)
    {
        return new Shape
        {
            Id = new Guid(line.Id.Value),
            Type = line.ShapeType,
            WhiteboardId = new Guid(line.WhiteboardId.IdValue),
            PositionX = line.Position.X,
            PositionY = line.Position.Y,
            Color = line.Color.Value,
            //SPECIFIC
            EndPositionX = line.EndPosition.X,
            EndPositionY = line.EndPosition.Y,
            Thickness = line.Thickness.Value,
        };
    }

    public static Shape ArrowToEntity(Arrow arrow)
    {
        return new Shape
        {
            Id = new Guid(arrow.Id.Value),
            Type = arrow.ShapeType,
            WhiteboardId = new Guid(arrow.WhiteboardId.IdValue),
            PositionX = arrow.Position.X,
            PositionY = arrow.Position.Y,
            Color = arrow.Color.Value,
            //SPECIFIC
            EndPositionX = arrow.EndPosition.X,
            EndPositionY = arrow.EndPosition.Y,
            Thickness = arrow.Thickness.Value,
        };
    }

    public static Shape TextShapeToEntity(TextShape textShape)
    {
        return new Shape
        {
            Id = new Guid(textShape.Id.Value),
            Type = textShape.ShapeType,
            WhiteboardId = new Guid(textShape.WhiteboardId.IdValue),
            PositionX = textShape.Position.X,
            PositionY = textShape.Position.Y,
            Color = textShape.Color.Value,
            //SPECIFIC
            TextValue = textShape.TextShapeValue.Text,
            TextSize = textShape.TextShapeSize.Size,
        };
    }
}