using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Domain.Common.ValueObjects;
using AipsTestsUtility;

namespace AipsNUnitTests.Domain.Common.ValueObjects;

[TestFixture]
public class ColorTests
{
    [TestCase("#FF5733")]
    [TestCase("#5733FF")]
    public void Constructor_WithValidFormat_ShouldCreateInstance(string validColor)
    {
        Assert.DoesNotThrow(() => _ = new Color(validColor));
    }

    [TestCase("")]
    [TestCase("#F335")]
    [TestCase("#FF5733FF")]
    [TestCase("#FF53nn")]
    [TestCase("$FF57335")]
    public void Constructor_WithInvalidFormat_ShouldBreakColorFormatRule(string invalidColor)
    {
        var exception = Assert.Throws<ValidationException>(() => _ = new Color(invalidColor));
        
        AssertUtility.AssertHasBrokenExactRule<ColorFormatRule>(exception);
    }
    
    [Test]
    public void Equals_WithSameColorValue_ShouldReturnTrue()
    {
        var color1 = new Color("#FF5733");
        var color2 = new Color("#FF5733");
        
        Assert.That(color1, Is.EqualTo(color2));
    }
    
    [Test]
    public void Equals_WithDifferentColorValue_ShouldReturnFalse()
    {
        var color1 = new Color("#FF5733");
        var color2 = new Color("#3357FF");
        
        Assert.That(color1, Is.Not.EqualTo(color2));
    }
}