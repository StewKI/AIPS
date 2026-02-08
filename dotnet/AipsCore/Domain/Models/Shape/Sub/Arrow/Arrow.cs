using System.Drawing;
using AipsCore.Domain.Models.Shape.Enums;
using AipsCore.Domain.Models.Shape.ValueObjects;

namespace AipsCore.Domain.Models.Shape.Sub.Arrow;

public class Arrow : Shape
{
    public Position EndPosition { get; private set; }
    public Thickness Thickness { get; private set; }
    
    public Arrow(ShapeId id, Position position, Color color, Position endPosition, Thickness thickness) : base(id, position, color)
    {
        EndPosition = endPosition;
        Thickness = thickness;
    }

    public override ShapeTypeEnum ShapeType => ShapeTypeEnum.Arrow;
}