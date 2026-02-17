using AipsRT.Model.Whiteboard.Structs;

namespace AipsRT.Model.Whiteboard.Shapes.Map;

public static class ShapeMappingExtensions
{
    extension(AipsCore.Infrastructure.Persistence.Shape.Shape shape)
    {
        public Rectangle ToRectangle()
        {
            return new Rectangle()
            {
                Id = shape.Id,
                OwnerId = shape.AuthorId,
                Position = new Position(shape.PositionX, shape.PositionY),
                Color = shape.Color,
                EndPosition = new Position(shape.EndPositionX!.Value, shape.EndPositionY!.Value),
                BorderThickness = shape.Thickness!.Value,
            };
        }

        public Arrow ToArrow()
        {
            return new Arrow()
            {
                Id = shape.Id,
                OwnerId = shape.AuthorId,
                Position = new Position(shape.PositionX, shape.PositionY),
                Color = shape.Color,
                EndPosition = new Position(shape.EndPositionX!.Value, shape.EndPositionY!.Value),
                Thickness = shape.Thickness!.Value,
            };
        }
        
        public Line ToLine()
        {
            return new Line()
            {
                Id = shape.Id,
                OwnerId = shape.AuthorId,
                Position = new Position(shape.PositionX, shape.PositionY),
                Color = shape.Color,
                EndPosition = new Position(shape.EndPositionX!.Value, shape.EndPositionY!.Value),
                Thickness = shape.Thickness!.Value,
            };
        }
        
        public TextShape ToTextShape()
        {
            return new TextShape()
            {
                Id = shape.Id,
                OwnerId = shape.AuthorId,
                Position = new Position(shape.PositionX, shape.PositionY),
                Color = shape.Color,
                TextValue = shape.TextValue!,
                TextSize = shape.TextSize!.Value
            };
        }
    }
}