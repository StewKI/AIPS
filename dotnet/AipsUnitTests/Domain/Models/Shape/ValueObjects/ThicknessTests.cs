using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Domain.Models.Shape.ValueObjects;
using AipsTestsUtility;

namespace AipsNUnitTests.Domain.Models.Shape.ValueObjects;

[TestFixture]
public class ThicknessTests
{
    [TestCase(5)]
    public void Constructor_WithValidThickness_ShouldCreateThickness(int validThickness)
    {
        Assert.DoesNotThrow(() => _ = new Thickness(validThickness));
    }
    
    [TestCase(0)]
    [TestCase(-1)]
    public void Constructor_WithThicknessTooLow_ShouldBreakMinThicknessRule(int tooLowThickness)
    {
        var exception = Assert.Throws<ValidationException>(() => _ = new Thickness(tooLowThickness));
        
        AssertUtility.AssertHasBrokenExactRule<MinValueRule<int>>(exception);
    }
    
    [TestCase(100)]
    public void Constructor_WithThicknessTooHigh_ShouldBreakMaxThicknessRule(int tooHighThickness)
    {
        var exception = Assert.Throws<ValidationException>(() => _ = new Thickness(tooHighThickness));
        
        AssertUtility.AssertHasBrokenExactRule<MaxValueRule<int>>(exception);
    }
    
    [Test]
    public void Equals_WithSameThicknessValue_ShouldReturnTrue()
    {
        var thickness1 = new Thickness(5);
        var thickness2 = new Thickness(5);
        
        Assert.That(thickness1, Is.EqualTo(thickness2));
    }
    
    [Test]
    public void Equals_WithDifferentThicknessValue_ShouldReturnFalse()
    {
        var thickness1 = new Thickness(5);
        var thickness2 = new Thickness(6);
        
        Assert.That(thickness1, Is.Not.EqualTo(thickness2));       
    }  
}