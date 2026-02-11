using AipsCore.Domain.Abstract;
using AipsCore.Domain.Common.ValueObjects;
using AipsCore.Domain.Models.Shape.Enums;
using AipsCore.Domain.Models.Shape.ValueObjects;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Domain.Models.Shape;

public abstract class Shape : DomainModel<ShapeId>
{
    public WhiteboardId WhiteboardId { get; private set; }
    
    public UserId AuthorId { get; private set; }
    
    public abstract ShapeType ShapeType { get; }
    
    public Position Position { get; private set; }

    public Color Color { get; private set; }

    protected Shape(ShapeId id, WhiteboardId whiteboardId, UserId authorId, Position position, Color color)
        : base(id)
    {
        Position = position;
        Color = color;
        AuthorId = authorId;
        WhiteboardId = whiteboardId;
    }

    protected Shape(
        string id,
        string whiteboardId,
        int positionX, int positionY,
        string color, UserId authorId)
    {
        Id = new ShapeId(id);
        Position = new Position(positionX, positionY);
        Color = new Color(color);
        AuthorId = authorId;
        WhiteboardId = new WhiteboardId(whiteboardId);
    }
}