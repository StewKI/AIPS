using System.Drawing;
using AipsCore.Domain.Models.Shape.Enums;
using AipsCore.Domain.Models.Shape.ValueObjects;

namespace AipsCore.Domain.Models.Shape.Sub.Rectangle;

public class Rectangle : Shape
{
    public override ShapeTypeEnum ShapeType => ShapeTypeEnum.Rectangle;
    
    public Position EndPosition { get; }
    
    public Thickness BorderThickness { get; }
    
    public Rectangle(ShapeId id, Position position, Color color, Position endPosition, Thickness borderThickness) 
        : base(id, position, color)
    {
        EndPosition = endPosition;
        BorderThickness = borderThickness;
    }

}