using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Domain.Models.WhiteboardMembership.ValueObjects;
using AipsTestsUtility;

namespace AipsNUnitTests.Domain.Models.WhiteboardMembership.ValueObjects;

[TestFixture]
public class WhiteboardMembershipLastInteractedAtTests
{
    [Test]
    public void Constructor_WithValidDate_ShouldCreateDate()
    {
        DateTime validDate = DateTime.Now.AddHours(-1);
        
        Assert.DoesNotThrow(() => _ = new WhiteboardMembershipLastInteractedAt(validDate));
    }

    [Test]
    public void Constructor_WithInvalidDate_ShouldBreakDateInPastRule()
    {
        DateTime invalidDate = DateTime.Now.AddHours(1);
        
        var exception = Assert.Throws<ValidationException>(() => _ = new WhiteboardMembershipLastInteractedAt(invalidDate));
        
        AssertUtility.AssertHasBrokenExactRule<DateInPastRule>(exception);
    }
    
    [Test]
    public void Equals_WithSameDateValue_ShouldReturnTrue()
    {
        DateTime validDate1 = DateTime.Now.AddHours(-1);
        DateTime validDate2 = validDate1;
        
        WhiteboardMembershipLastInteractedAt date1 = new WhiteboardMembershipLastInteractedAt(validDate1);
        WhiteboardMembershipLastInteractedAt date2 = new WhiteboardMembershipLastInteractedAt(validDate2);
        
        Assert.That(date1, Is.EqualTo(date2));
    }
    
    [Test]
    public void Equals_WithDifferentDateValue_ShouldReturnFalse()
    {
        DateTime validDate1 = DateTime.Now.AddHours(-1);
        DateTime validDate2 = DateTime.Now.AddHours(-2);
        
        WhiteboardMembershipLastInteractedAt date1 = new WhiteboardMembershipLastInteractedAt(validDate1);
        WhiteboardMembershipLastInteractedAt date2 = new WhiteboardMembershipLastInteractedAt(validDate2);
        
        Assert.That(date1, Is.Not.EqualTo(date2));       
    }
}