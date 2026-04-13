using TextShapeModel = AipsCore.Domain.Models.Shape.Sub.TextShape.TextShape;

namespace AipsNUnitTests.Domain.Models.Shape.Sub.TextShape;

[TestFixture]
public class TextShapeTests
{
    [Test]
    public void Create_WithValidData_ShouldInitializeTextShapePropertiesCorrectly()
    {
        var whiteboardId = Guid.NewGuid().ToString();
        var authorId = Guid.NewGuid().ToString();
        var positionX = 10;
        var positionY = 20;
        var color = "#FF0000";
        var value = "Test TextShape value 123456789.";
        var size = 16;
        
        var textShape = TextShapeModel.Create(
            whiteboardId, 
            authorId, 
            positionX, 
            positionY, 
            color, 
            value, 
            size);
        
        Assert.Multiple(() =>
        {
            Assert.That(textShape.Id, Is.Not.Null);
            Assert.That(textShape.WhiteboardId.IdValue, Is.EqualTo(whiteboardId));
            Assert.That(textShape.AuthorId.IdValue, Is.EqualTo(authorId));
            Assert.That(textShape.Position.X, Is.EqualTo(positionX));
            Assert.That(textShape.Position.Y, Is.EqualTo(positionY));
            Assert.That(textShape.Color.Value, Is.EqualTo(color));
            Assert.That(textShape.TextShapeValue.Text, Is.EqualTo(value));
            Assert.That(textShape.TextShapeSize.Size, Is.EqualTo(size));
        });
    }
}