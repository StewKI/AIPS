using RectangleModel = AipsCore.Domain.Models.Shape.Sub.Rectangle.Rectangle;

namespace AipsNUnitTests.Domain.Models.Shape.Sub.Rectangle;

[TestFixture]
public class RectangleTests
{
    [Test]
    public void Create_WithValidData_ShouldInitializeLinePropertiesCorrectly()
    {
        var whiteboardId = Guid.NewGuid().ToString();
        var authorId = Guid.NewGuid().ToString();
        var positionX = 10;
        var positionY = 20;
        var color = "#FF0000";
        var endPositionX = 30;
        var endPositionY = 30;
        var borderThickness = 5;

        var rectangle = RectangleModel.Create(
            whiteboardId,
            authorId,
            positionX,
            positionY,
            color,
            endPositionX,
            endPositionY,
            borderThickness);

        Assert.Multiple(() =>
        {
            Assert.That(rectangle.Id, Is.Not.Null);
            Assert.That(rectangle.WhiteboardId.IdValue, Is.EqualTo(whiteboardId));
            Assert.That(rectangle.AuthorId.IdValue, Is.EqualTo(authorId));
            Assert.That(rectangle.Position.X, Is.EqualTo(positionX));
            Assert.That(rectangle.Position.Y, Is.EqualTo(positionY));
            Assert.That(rectangle.Color.Value, Is.EqualTo(color));
            Assert.That(rectangle.EndPosition.X, Is.EqualTo(endPositionX));
            Assert.That(rectangle.EndPosition.Y, Is.EqualTo(endPositionY));
            Assert.That(rectangle.BorderThickness.Value, Is.EqualTo(borderThickness));
        });
    }
}