using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Domain.Common.ValueObjects;
using AipsTestsUtility;

namespace AipsNUnitTests.Domain.Common.ValueObjects;

[TestFixture]
public class DeletedAtTests
{
    [Test]
    public void Constructor_WithValidDate_ShouldCreateDate()
    {
        DateTime validDate = DateTime.Now.AddHours(-1);
        
        Assert.DoesNotThrow(() => _ = new DeletedAt(validDate));
    }

    [Test]
    public void Constructor_WithInvalidDate_ShouldBreakDateInPastRule()
    {
        DateTime invalidDate = DateTime.Now.AddHours(1);
        
        var exception = Assert.Throws<ValidationException>(() => _ = new DeletedAt(invalidDate));
        
        AssertUtility.AssertHasBrokenExactRule<DateInPastRule>(exception);
    }
    
    [Test]
    public void Equals_WithSameDateValue_ShouldReturnTrue()
    {
        DateTime date1 = DateTime.Now.AddHours(-1);
        DateTime date2 = date1;
        
        DeletedAt date1Vo = new DeletedAt(date1);
        DeletedAt date2Vo = new DeletedAt(date2);
        
        Assert.That(date1Vo, Is.EqualTo(date2Vo));
    }
    
    [Test]
    public void Equals_WithDifferentDateValue_ShouldReturnFalse()
    {
        DateTime date1 = DateTime.Now.AddHours(-1);
        DateTime date2 = DateTime.Now.AddHours(-2);
        
        DeletedAt date1Vo = new DeletedAt(date1);
        DeletedAt date2Vo = new DeletedAt(date2);
        
        Assert.That(date1Vo, Is.Not.EqualTo(date2Vo));       
    }
}