using AipsRT.Model.Whiteboard.Structs;

namespace AipsRT.Model.Whiteboard.Shapes;

public class Rectangle : Shape
{
    public Position EndPosition { get; set; }
    
    public int BorderThickness { get; set; }

    public override void Move(Position newPosition)
    {
        var difference = newPosition - Position;
        EndPosition += difference;
        base.Move(newPosition);
    }
}