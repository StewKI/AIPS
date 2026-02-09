using AipsCore.Domain.Models.Shape.Enums;
using AipsCore.Domain.Models.Shape.Sub.Arrow;
using AipsCore.Domain.Models.Shape.Sub.Line;
using AipsCore.Domain.Models.Shape.Sub.Rectangle;
using AipsCore.Domain.Models.Shape.Sub.TextShape;

namespace AipsCore.Infrastructure.Persistence.Shape;

public static class ShapeMappers
{
    #region FROM_ENTITY
    public static Domain.Models.Shape.Shape EntityToModel(Shape shape)
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
            shape.PositionX, shape.PositionY,
            shape.Color,
            shape.TextValue!, shape.TextSize!.Value);
    }
    #endregion
    
    #region TO_ENTITY

    public static Shape ModelToEntity(Domain.Models.Shape.Shape model)
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
        return new Shape()
        {
            Id = new Guid(rectangle.Id.Value),
            Type = rectangle.ShapeType,
            WhiteboardId = new Guid(rectangle.WhiteboardId.IdValue),
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
        return new Shape()
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
        return new Shape()
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
        return new Shape()
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
        
    #endregion
    
    #region UPDATE_ENTITY

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
    
    #endregion
}