using System.Drawing;
using AipsCore.Domain.Models.Shape.Enums;
using AipsCore.Domain.Models.Shape.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Domain.Models.Shape.Sub.Line;

public class Line : Shape
{
    public Position EndPosition { get; private set; }
    public Thickness Thickness { get; private set; }
    
    public Line(ShapeId id, WhiteboardId whiteboardId, Position position, Color color, Position endPosition, Thickness thickness) : base(id, whiteboardId, position, color)
    {
        EndPosition = endPosition;
        Thickness = thickness;
    }

    public override ShapeTypeEnum ShapeType =>  ShapeTypeEnum.Line;
}