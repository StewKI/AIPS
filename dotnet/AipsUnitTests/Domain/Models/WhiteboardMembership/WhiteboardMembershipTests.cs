using AipsCore.Domain.Models.WhiteboardMembership.Enums;
using WhiteboardMembershipModel = AipsCore.Domain.Models.WhiteboardMembership.WhiteboardMembership;

namespace AipsNUnitTests.Domain.Models.WhiteboardMembership;

[TestFixture]
public class WhiteboardMembershipTests
{
    [Test]
    public void Create_WithValidData_ShouldInitializeWhiteboardMembershipPropertiesCorrectly()
    {
        var whiteboardId = Guid.NewGuid().ToString();
        var userId = Guid.NewGuid().ToString();
        var editingEnabled = true;
        var status = WhiteboardMembershipStatus.Accepted;
        var lastInteractedAt = DateTime.Now;
        
        var whiteboardMembership = WhiteboardMembershipModel.Create(
            whiteboardId, 
            userId, 
            editingEnabled, 
            status, 
            lastInteractedAt);
        
        Assert.Multiple(() =>
        {
            Assert.That(whiteboardMembership.Id, Is.Not.Null);
            Assert.That(whiteboardMembership.WhiteboardId.IdValue, Is.EqualTo(whiteboardId));
            Assert.That(whiteboardMembership.UserId.IdValue, Is.EqualTo(userId));
            Assert.That(whiteboardMembership.EditingEnabled.EditingEnabledValue, Is.EqualTo(editingEnabled));
            Assert.That(whiteboardMembership.Status, Is.EqualTo(status));
            Assert.That(whiteboardMembership.LastInteractedAt.LastInteractedAtValue, Is.EqualTo(lastInteractedAt));
        });
    }
}