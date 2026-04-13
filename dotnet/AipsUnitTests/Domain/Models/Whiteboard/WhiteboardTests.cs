using AipsCore.Domain.Models.Whiteboard.Enums;
using WhiteboardModel = AipsCore.Domain.Models.Whiteboard.Whiteboard;

namespace AipsNUnitTests.Domain.Models.Whiteboard;

[TestFixture]
public class WhiteboardTests
{
    [Test]
    public void Create_WithValidData_ShouldInitializeWhiteboardPropertiesCorrectly()
    {
        var ownerId = Guid.NewGuid().ToString();
        var title = "Test Title";
        var code = "12345678";
        var maxParticipants = 10;
        
        var whiteboard = WhiteboardModel.Create(
            ownerId, 
            code, 
            title, 
            maxParticipants, 
            WhiteboardJoinPolicy.FreeToJoin);
        
        Assert.Multiple(() =>
        {
            Assert.That(whiteboard.Id, Is.Not.Null);
            Assert.That(whiteboard.WhiteboardOwnerId.IdValue, Is.EqualTo(ownerId));
            Assert.That(whiteboard.Code.CodeValue, Is.EqualTo(code));
            Assert.That(whiteboard.Title.TitleValue, Is.EqualTo(title));
            Assert.That(whiteboard.CreatedAt, Is.Not.Null);
            Assert.That(whiteboard.DeletedAt.DeletedAtValue, Is.Null);
            Assert.That(whiteboard.MaxParticipants.MaxParticipantsValue, Is.EqualTo(maxParticipants));
            Assert.That(whiteboard.JoinPolicy, Is.EqualTo(WhiteboardJoinPolicy.FreeToJoin));
            Assert.That(whiteboard.State, Is.EqualTo(WhiteboardState.Active));
        });
    }
}