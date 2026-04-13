using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Domain.Models.Shape.Sub.TextShape.ValueObjects;
using AipsTestsUtility;

namespace AipsNUnitTests.Domain.Models.Shape.Sub.TextShape.ValueObjects;

[TestFixture]
public class TextShapeSizeTests
{
    [TestCase(12)]
    public void Constructor_WithValidSize_ShouldCreateTextShapeSize(int validSize)
    {
        Assert.DoesNotThrow(() => _ = new TextShapeSize(validSize));
    }

    [TestCase(7)]
    [TestCase(-1)]
    public void Constructor_WithSizeTooLow_ShouldBreakTextShapeMinSizeRule(int sizeTooLow)
    {
        var exception = Assert.Throws<ValidationException>(() => _ = new TextShapeSize(sizeTooLow));
        
        AssertUtility.AssertHasBrokenExactRule<MinValueRule<int>>(exception);
    }
    
    
    [TestCase(73)]
    public void Constructor_WithSizeTooBig_ShouldBreakTextShapeMaxSizeRule(int sizeTooBig)
    {
        var exception = Assert.Throws<ValidationException>(() => _ = new TextShapeSize(sizeTooBig));
        
        AssertUtility.AssertHasBrokenExactRule<MaxValueRule<int>>(exception);
    }
    
    [Test]
    public void Equals_WithSameSizeValue_ShouldReturnTrue()
    {
        var size1 = new TextShapeSize(12);
        var size2 = new TextShapeSize(12);
        
        Assert.That(size1, Is.EqualTo(size2));
    }

    [Test]
    public void Equals_WithDifferentSizeValue_ShouldReturnFalse()
    {
        var size1 = new TextShapeSize(12);
        var size2 = new TextShapeSize(13);
        
        Assert.That(size1, Is.Not.EqualTo(size2));      
    }
}