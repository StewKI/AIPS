using AipsRT.Model.Whiteboard.Structs;

namespace AipsRT.Model.Whiteboard.Shapes;

public abstract class Shape
{
    public Guid Id { get; set; }
    
    public Guid OwnerId { get; set; }

    public Position Position { get; set; }
    
    public string Color { get; set; }
}