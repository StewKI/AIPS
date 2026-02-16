using AipsRT.Model.Whiteboard.Shapes;

namespace AipsRT.Model.Whiteboard.Shapes;

public class TextShape : Shape
{
    public string TextValue { get; set; }
    
    public int TextSize { get; set; }
}