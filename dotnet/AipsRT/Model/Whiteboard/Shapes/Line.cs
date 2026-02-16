using AipsRT.Model.Whiteboard.Structs;

namespace AipsRT.Model.Whiteboard.Shapes;

public class Line : Shape
{
    public Position EndPosition { get; set; }
    
    public int Thickness { get; set; }
}