using AipsCore.Domain.Common.ValueObjects;
using AipsCore.Domain.Models.Shape.Enums;
using AipsCore.Domain.Models.Shape.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Domain.Models.Shape.Sub.Rectangle;

public class Rectangle : Shape
{
    public override ShapeType ShapeType => ShapeType.Rectangle;
    
    public Position EndPosition { get; }
    
    public Thickness BorderThickness { get; }
    
    public Rectangle(ShapeId id, WhiteboardId whiteboardId, Position position, Color color, Position endPosition, Thickness borderThickness) 
        : base(id, whiteboardId, position, color)
    {
        EndPosition = endPosition;
        BorderThickness = borderThickness;
    }

}