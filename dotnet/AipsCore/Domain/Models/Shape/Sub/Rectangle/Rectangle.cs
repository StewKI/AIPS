using AipsCore.Domain.Common.ValueObjects;
using AipsCore.Domain.Models.Shape.Enums;
using AipsCore.Domain.Models.Shape.ValueObjects;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Domain.Models.Shape.Sub.Rectangle;

public class Rectangle : Shape
{
    public override ShapeType ShapeType => ShapeType.Rectangle;
    
    public Position EndPosition { get; }
    
    public Thickness BorderThickness { get; }
    
    public Rectangle(ShapeId id, WhiteboardId whiteboardId, UserId authorId, Position position, Color color, Position endPosition, Thickness borderThickness) 
        : base(id, whiteboardId, authorId, position, color)
    {
        EndPosition = endPosition;
        BorderThickness = borderThickness;
    }

    public static Rectangle Create(
        string id,
        string whiteboardId,
        string authorId,
        int positionX, int positionY,
        string color,
        int endPositionX, int endPositionY,
        int borderThickness)
    {
        return new Rectangle(
            new ShapeId(id),
            new WhiteboardId(whiteboardId),
            new UserId(authorId),
            new Position(positionX, positionY),
            new Color(color),
            new Position(endPositionX, endPositionY),
            new Thickness(borderThickness));
    }
    
    public static Rectangle Create(
        string whiteboardId,
        string authorId,
        int positionX, int positionY,
        string color,
        int endPositionX, int endPositionY,
        int borderThickness)
    {
        return new Rectangle(
            ShapeId.Any(),
            new WhiteboardId(whiteboardId),
            new UserId(authorId),
            new Position(positionX, positionY),
            new Color(color),
            new Position(endPositionX, endPositionY),
            new Thickness(borderThickness));
    }
}