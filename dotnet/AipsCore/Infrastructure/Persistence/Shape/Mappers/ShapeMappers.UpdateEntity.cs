using AipsCore.Domain.Models.Shape.Enums;
using AipsCore.Domain.Models.Shape.Sub.Arrow;
using AipsCore.Domain.Models.Shape.Sub.Line;
using AipsCore.Domain.Models.Shape.Sub.Rectangle;
using AipsCore.Domain.Models.Shape.Sub.TextShape;

namespace AipsCore.Infrastructure.Persistence.Shape.Mappers;

public static partial class ShapeMappers
{
    public static void UpdateEntity(Shape entity, Domain.Models.Shape.Shape model)
    {
        entity.WhiteboardId = new Guid(model.WhiteboardId.IdValue);
        entity.PositionX = model.Position.X;
        entity.PositionY = model.Position.Y;
        entity.Color = model.Color.Value;

        switch (model.ShapeType)
        {
            case ShapeType.Rectangle:
                UpdateEntityFromRectangle(entity, (Rectangle)model);
                break;
            case ShapeType.Line:
                UpdateEntityFromLine(entity, (Line)model);
                break;
            case ShapeType.Arrow:
                UpdateEntityFromArrow(entity, (Arrow)model);
                break;
            case ShapeType.Text:
                UpdateEntityFromTextShape(entity, (TextShape)model);
                break;
        };
    }

    public static void UpdateEntityFromRectangle(Shape entity, Rectangle rectangle)
    {
        entity.EndPositionX = rectangle.EndPosition.X;
        entity.EndPositionY = rectangle.EndPosition.Y;
        entity.Thickness = rectangle.BorderThickness.Value;
    }

    public static void UpdateEntityFromLine(Shape entity, Line line)
    {
        entity.EndPositionX = line.EndPosition.X;
        entity.EndPositionY = line.EndPosition.Y;
        entity.Thickness = line.Thickness.Value;
    }

    public static void UpdateEntityFromArrow(Shape entity, Arrow arrow)
    {
        entity.EndPositionX = arrow.EndPosition.X;
        entity.EndPositionY = arrow.EndPosition.Y;
        entity.Thickness = arrow.Thickness.Value;
    }

    public static void UpdateEntityFromTextShape(Shape entity, TextShape textShape)
    {
        entity.TextValue = textShape.TextShapeValue.Text;
        entity.TextSize = textShape.TextShapeSize.Size;
    }
}