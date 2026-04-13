using LineModel = AipsCore.Domain.Models.Shape.Sub.Line.Line;

namespace AipsNUnitTests.Domain.Models.Shape.Sub.Line;

[TestFixture]
public class LineTests
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
        var thickness = 5;
        
        var line = LineModel.Create(
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
            Assert.That(line.Id, Is.Not.Null);
            Assert.That(line.WhiteboardId.IdValue, Is.EqualTo(whiteboardId));
            Assert.That(line.AuthorId.IdValue, Is.EqualTo(authorId));
            Assert.That(line.Position.X, Is.EqualTo(positionX));
            Assert.That(line.Position.Y, Is.EqualTo(positionY));
            Assert.That(line.Color.Value, Is.EqualTo(color));
            Assert.That(line.EndPosition.X, Is.EqualTo(endPositionX));
            Assert.That(line.EndPosition.Y, Is.EqualTo(endPositionY));
            Assert.That(line.Thickness.Value, Is.EqualTo(thickness));
        });
    }
}