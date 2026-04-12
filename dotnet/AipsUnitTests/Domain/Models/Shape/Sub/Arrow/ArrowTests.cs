using ArrowModel = AipsCore.Domain.Models.Shape.Sub.Arrow.Arrow;

namespace AipsNUnitTests.Domain.Models.Shape.Sub.Arrow;

[TestFixture]
public class ArrowTests
{
    [Test]
    public void Create_WithValidData_ShouldInitializeArrowPropertiesCorrectly()
    {
        var whiteboardId = Guid.NewGuid().ToString();
        var authorId = Guid.NewGuid().ToString();
        var positionX = 10;
        var positionY = 20;
        var color = "#FF0000";
        var endPositionX = 30;
        var endPositionY = 40;
        var thickness = 5;
        
        var arrow = ArrowModel.Create(
            whiteboardId, 
            authorId, 
            positionX, 
            positionY, 
            color, 
            endPositionX, 
            endPositionY, 
            thickness);
        
        Assert.Multiple(() =>
        {
            Assert.That(arrow.Id, Is.Not.Null);
            Assert.That(arrow.WhiteboardId.IdValue, Is.EqualTo(whiteboardId));
            Assert.That(arrow.AuthorId.IdValue, Is.EqualTo(authorId));
            Assert.That(arrow.Position.X, Is.EqualTo(positionX));
            Assert.That(arrow.Position.Y, Is.EqualTo(positionY));
            Assert.That(arrow.Color.Value, Is.EqualTo(color));
            Assert.That(arrow.EndPosition.X, Is.EqualTo(endPositionX));
            Assert.That(arrow.EndPosition.Y, Is.EqualTo(endPositionY));
            Assert.That(arrow.Thickness.Value, Is.EqualTo(thickness));
        });
    }
}