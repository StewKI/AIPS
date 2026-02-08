using System.Drawing;
using AipsCore.Domain.Models.Shape.Enums;
using AipsCore.Domain.Models.Shape.ValueObjects;

namespace AipsCore.Domain.Models.Shape;

public abstract class Shape
{
    public ShapeId Id { get; }
    
    public abstract ShapeTypeEnum ShapeType { get; }
    
    public Position Position { get; private set; }

    public Color Color { get; private set; }

    protected Shape(ShapeId id, Position position, Color color)
    {
        Id = id;
        Position = position;
        Color = color;
    }
}