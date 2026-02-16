using AipsCore.Domain.Common.ValueObjects;
using AipsCore.Domain.Models.Shape.Enums;
using AipsCore.Domain.Models.Shape.ValueObjects;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Domain.Models.Shape.Sub.Line;

public partial class Line : Shape
{
    public Position EndPosition { get; private set; }
    public Thickness Thickness { get; private set; }
    
    public Line(ShapeId id, WhiteboardId whiteboardId, UserId authorId, Position position, Color color, Position endPosition, Thickness thickness) 
        : base(id, whiteboardId, authorId, position, color)
    {
        EndPosition = endPosition;
        Thickness = thickness;
    }

    public override ShapeType ShapeType =>  ShapeType.Line;
    
    public static Line Create(
        string id,
        string whiteboardId,
        string authorId,
        int positionX, int positionY,
        string color,
        int endPositionX, int endPositionY,
        int borderThickness)
    {
        return new Line(
            new ShapeId(id),
            new WhiteboardId(whiteboardId),
            new UserId(authorId),
            new Position(positionX, positionY),
            new Color(color),
            new Position(endPositionX, endPositionY),
            new Thickness(borderThickness));
    }
    
    public static Line Create(
        string whiteboardId,
        string authorId,
        int positionX, int positionY,
        string color,
        int endPositionX, int endPositionY,
        int borderThickness)
    {
        return new Line(
            ShapeId.Any(),
            new WhiteboardId(whiteboardId),
            new UserId(authorId),
            new Position(positionX, positionY),
            new Color(color),
            new Position(endPositionX, endPositionY),
            new Thickness(borderThickness));
    }
}