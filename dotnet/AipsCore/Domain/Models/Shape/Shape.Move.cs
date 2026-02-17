using AipsCore.Domain.Models.Shape.ValueObjects;

namespace AipsCore.Domain.Models.Shape;

public partial class Shape
{
    public virtual void Move(int newPositionX, int newPositionY)
    {
        var newPosition = new Position(newPositionX, newPositionY);
        
        this.Position = newPosition;
    }
}