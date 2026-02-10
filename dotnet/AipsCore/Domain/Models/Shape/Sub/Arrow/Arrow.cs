using AipsCore.Domain.Common.ValueObjects;
using AipsCore.Domain.Models.Shape.Enums;
using AipsCore.Domain.Models.Shape.ValueObjects;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Domain.Models.Shape.Sub.Arrow;

public class Arrow : Shape
{
    public Position EndPosition { get; private set; }
    public Thickness Thickness { get; private set; }
    
    public Arrow(ShapeId id, WhiteboardId whiteboardId, UserId authorId, Position position, Color color, Position endPosition, Thickness thickness)
        : base(id, whiteboardId, authorId, position, color)
    {
        EndPosition = endPosition;
        Thickness = thickness;
    }

    public override ShapeType ShapeType => ShapeType.Arrow;
    
    public static Arrow Create(
        string id,
        string whiteboardId,
        string authorId,
        int positionX, int positionY,
        string color,
        int endPositionX, int endPositionY,
        int borderThickness)
    {
        return new Arrow(
            new ShapeId(id),
            new WhiteboardId(whiteboardId),
            new UserId(authorId),
            new Position(positionX, positionY),
            new Color(color),
            new Position(endPositionX, endPositionY),
            new Thickness(borderThickness));
    }
}