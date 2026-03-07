using AipsRT.Model.Whiteboard.Shapes;
using AipsRT.Model.Users;

namespace AipsRT.Model.Whiteboard;

public class Whiteboard
{
    public Guid WhiteboardId { get; set; }
    
    public Guid OwnerId { get; set; }
    
    public HashSet<Guid> AcceptedUsers { get; } = new();
    public HashSet<Guid> PendingUsers { get; } = new();
    
    public List<User> Users { get; } = [];

    public List<Shape> Shapes { get; } = [];

    public List<Rectangle> Rectangles { get; } = [];
    public List<Arrow> Arrows { get; } = [];
    public List<Line> Lines { get; } = [];
    public List<TextShape> TextShapes { get; } = [];

    public void AddRectangle(Rectangle shape)
    {
        Shapes.Add(shape);
        Rectangles.Add(shape);
    }
    
    public void AddArrow(Arrow shape)
    {
        Shapes.Add(shape);
        Arrows.Add(shape);
    }
    
    public void AddLine(Line shape)
    {
        Shapes.Add(shape);
        Lines.Add(shape);
    }
    
    public void AddTextShape(TextShape shape)
    {
        Shapes.Add(shape);
        TextShapes.Add(shape);
    }
    
    public void AddUser(User user) => Users.Add(user);
    
    public void AddPendingUser(Guid userId) => PendingUsers.Add(userId);

    public void AcceptUser(Guid userId)
    {
        PendingUsers.Remove(userId);
        AcceptedUsers.Add(userId);
    }

    public void RejectUser(Guid userId) => PendingUsers.Remove(userId);

    public bool IsAccepted(Guid userId) => AcceptedUsers.Contains(userId);
}