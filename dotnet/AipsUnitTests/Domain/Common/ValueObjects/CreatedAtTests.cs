using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Domain.Common.ValueObjects;
using AipsTestsUtility;

namespace AipsNUnitTests.Domain.Common.ValueObjects;

[TestFixture]
public class CreatedAtTests
{
    [Test]
    public void Constructor_WithValidDate_ShouldCreateDate()
    {
        DateTime validDate = DateTime.Now.AddHours(-1);
        
        Assert.DoesNotThrow(() => _ = new CreatedAt(validDate));
    }

    [Test]
    public void Constructor_WithInvalidDate_ShouldBreakDateInPastRule()
    {
        DateTime invalidDate = DateTime.Now.AddHours(1);
        
        var exception = Assert.Throws<ValidationException>(() => _ = new CreatedAt(invalidDate));
        
        AssertUtility.AssertHasBrokenExactRule<DateInPastRule>(exception);
    }
    
    [Test]
    public void Equals_WithSameDateValue_ShouldReturnTrue()
    {
        DateTime validDate1 = DateTime.Now.AddHours(-1);
        DateTime validDate2 = validDate1;
        
        CreatedAt date1 = new CreatedAt(validDate1);
        CreatedAt date2 = new CreatedAt(validDate2);
        
        Assert.That(date1, Is.EqualTo(date2));
    }
    
    [Test]
    public void Equals_WithDifferentDateValue_ShouldReturnFalse()
    {
        DateTime validDate1 = DateTime.Now.AddHours(-1);
        DateTime validDate2 = DateTime.Now.AddHours(-2);
        
        CreatedAt date1 = new CreatedAt(validDate1);
        CreatedAt date2 = new CreatedAt(validDate2);
        
        Assert.That(date1, Is.Not.EqualTo(date2));       
    }
}