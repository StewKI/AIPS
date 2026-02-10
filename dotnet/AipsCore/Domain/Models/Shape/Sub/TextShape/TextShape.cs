using AipsCore.Domain.Common.ValueObjects;
using AipsCore.Domain.Models.Shape.Enums;
using AipsCore.Domain.Models.Shape.Sub.TextShape.ValueObjects;
using AipsCore.Domain.Models.Shape.ValueObjects;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Domain.Models.Shape.Sub.TextShape;

public class TextShape : Shape
{
    public TextShapeValue TextShapeValue { get; private set; }
    
    public TextShapeSize TextShapeSize { get; private set; }
    
    public TextShape(ShapeId id, WhiteboardId whiteboardId, UserId authorId, Position position, Color color, TextShapeValue textShapeValue, TextShapeSize textShapeSize) 
        : base(id, whiteboardId, authorId, position, color)
    {
        TextShapeValue = textShapeValue;
        TextShapeSize = textShapeSize;
    }

    public override ShapeType ShapeType => ShapeType.Text;
    
    public static TextShape Create(
        string id,
        string whiteboardId,
        string authorId,
        int positionX, int positionY,
        string color,
        string textValue, int textSize)
    {
        return new TextShape(
            new ShapeId(id),
            new WhiteboardId(whiteboardId),
            new UserId(authorId),
            new Position(positionX, positionY),
            new Color(color),
            new TextShapeValue(textValue),
            new TextShapeSize(textSize));
    }
}