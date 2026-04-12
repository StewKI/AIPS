using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsTestsUtility;

namespace AipsNUnitTests.Domain.Models.Whiteboard.ValueObjects;

[TestFixture]
public class WhiteboardMaxParticipantsTests
{
    [TestCase(10)]
    public void Constructor_WithValidMaxParticipants_ShouldCreateWhiteboardMaxParticipants(int validMaxParticipants)
    {
        Assert.DoesNotThrow(() => _ = new WhiteboardMaxParticipants(validMaxParticipants));
    }

    [TestCase(10000000)]
    public void Constructor_WithInvalidMaxParticipants_ShouldBreakWhiteboardMaxParticipantsRule(int tooMuchMaxParticipants)
    {
        var exception = Assert.Throws<ValidationException>(() => _ = new WhiteboardMaxParticipants(tooMuchMaxParticipants));

        AssertUtility.AssertHasBrokenExactRule<MaxValueRule<int>>(exception);
    }
    
    [TestCase(0)]
    [TestCase(-1)]
    public void Constructor_WithInvalidMinParticipants_ShouldBreakWhiteboardMinParticipantsRule(int notEnoughMaxParticipants)
    {
        var exception = Assert.Throws<ValidationException>(() => _ = new WhiteboardMaxParticipants(notEnoughMaxParticipants));

        AssertUtility.AssertHasBrokenExactRule<MinValueRule<int>>(exception);
    }
    
    [Test]
    public void Equals_WithSameMaxParticipantsValue_ShouldReturnTrue()
    {
        var maxParticipants1 = new WhiteboardMaxParticipants(10);
        var maxParticipants2 = new WhiteboardMaxParticipants(10);
        
        Assert.That(maxParticipants1, Is.EqualTo(maxParticipants2));
    }
    
    [Test]
    public void Equals_WithDifferentMaxParticipantsValue_ShouldReturnFalse()
    {
        var maxParticipants1 = new WhiteboardMaxParticipants(10);
        var maxParticipants2 = new WhiteboardMaxParticipants(20);
        
        Assert.That(maxParticipants1, Is.Not.EqualTo(maxParticipants2));       
    }  
}