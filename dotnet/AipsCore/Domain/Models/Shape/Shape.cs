using AipsCore.Domain.Common.ValueObjects;
using AipsCore.Domain.Models.Shape.Enums;
using AipsCore.Domain.Models.Shape.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Domain.Models.Shape;

public abstract class Shape
{
    public ShapeId Id { get; }
    
    public WhiteboardId WhiteboardId { get; private set; }
    
    public abstract ShapeType ShapeType { get; }
    
    public Position Position { get; private set; }

    public Color Color { get; private set; }

    protected Shape(ShapeId id, WhiteboardId whiteboardId, Position position, Color color)
    {
        Id = id;
        Position = position;
        Color = color;
        WhiteboardId = whiteboardId;
    }
}