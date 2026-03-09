using AipsRT.Model.Whiteboard.Shapes;
using AipsRT.Model.Users;

namespace AipsRT.Model.Whiteboard;

public class Whiteboard
{
    public Guid WhiteboardId { get; set; }

    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;
    
    public string Code {get; set;}

    public List<User> Users { get; } = [];

    public List<User> ActiveUsers { get; } = [];
    public void AddActiveUser(User user) => ActiveUsers.Add(user);
    public void RemoveActiveUser(Guid userId) => ActiveUsers.RemoveAll(u => u.UserId == userId);
    
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

    public void AddUser(User user)
    {
        if (!Users.Contains(user))
        {
            Users.Add(user);
        }
    }
}