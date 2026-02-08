using AipsCore.Domain.Common.ValueObjects;
using AipsCore.Domain.Models.Shape.Enums;
using AipsCore.Domain.Models.Shape.Sub.TextShape.ValueObjects;
using AipsCore.Domain.Models.Shape.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Domain.Models.Shape.Sub.TextShape;

public class TextShape : Shape
{
    public TextShapeValue TextShapeValue { get; private set; }
    
    public TextShapeSize TextShapeSize { get; private set; }
    
    public TextShape(ShapeId id, WhiteboardId whiteboardId, Position position, Color color, TextShapeValue textShapeValue, TextShapeSize textShapeSize) 
        : base(id, whiteboardId, position, color)
    {
        TextShapeValue = textShapeValue;
        TextShapeSize = textShapeSize;
    }

    public override ShapeTypeEnum ShapeType => ShapeTypeEnum.Text;
}