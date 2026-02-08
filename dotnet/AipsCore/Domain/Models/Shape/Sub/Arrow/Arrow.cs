using AipsCore.Domain.Common.ValueObjects;
using AipsCore.Domain.Models.Shape.Enums;
using AipsCore.Domain.Models.Shape.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Domain.Models.Shape.Sub.Arrow;

public class Arrow : Shape
{
    public Position EndPosition { get; private set; }
    public Thickness Thickness { get; private set; }
    
    public Arrow(ShapeId id, WhiteboardId whiteboardId, Position position, Color color, Position endPosition, Thickness thickness) : base(id, whiteboardId, position, color)
    {
        EndPosition = endPosition;
        Thickness = thickness;
    }

    public override ShapeType ShapeType => ShapeType.Arrow;
}