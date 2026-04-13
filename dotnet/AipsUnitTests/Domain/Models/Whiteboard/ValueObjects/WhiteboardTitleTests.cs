using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Domain.Models.Whiteboard.Validation.Rules;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsTestsUtility;

namespace AipsNUnitTests.Domain.Models.Whiteboard.ValueObjects;

[TestFixture]
public class WhiteboardTitleTests
{
    [TestCase("Test Title")]
    [TestCase("/v1 _")]
    public void Constructor_WithValidTitle_ShouldCreateWhiteboardTitle(string validTitle)
    {
        Assert.DoesNotThrow(() => _ = new WhiteboardTitle(validTitle));
    }

    [TestCase("")]
    [TestCase("Oo")]
    public void Constructor_WithTitleTooShort_ShouldBreakWhiteboardTitleMinLengthRule(string titleTooShort)
    {
        var exception = Assert.Throws<ValidationException>(() => _ = new WhiteboardTitle(titleTooShort));
        
        AssertUtility.AssertHasBrokenExactRule<MinLengthRule>(exception);
    }
    
    [TestCase("Test Title Test Title Test Title Test Title Test Title")]
    public void Constructor_WithTitleTooLong_ShouldBreakWhiteboardTitleMaxLengthRule(string titleTooLong)
    {
        var exception = Assert.Throws<ValidationException>(() => _ = new WhiteboardTitle(titleTooLong));
        
        AssertUtility.AssertHasBrokenExactRule<MaxLengthRule>(exception);
    }

    [TestCase("Test Title?")]
    public void Constructor_WithInvalidCharacters_ShouldBreakWhiteboardTitleCharsetRule(string invalidCharsetTitle)
    {
        var exception = Assert.Throws<ValidationException>(() => _ = new WhiteboardTitle(invalidCharsetTitle));
        
        AssertUtility.AssertHasBrokenExactRule<WhiteboardTitleCharsetRule>(exception);
    }
    
    [Test]
    public void Equals_WithSameTitleValue_ShouldReturnTrue()
    {
        var title1 = new WhiteboardTitle("Test Title");
        var title2 = new WhiteboardTitle("Test Title");
        
        Assert.That(title1, Is.EqualTo(title2));
    }
    
    [Test]
    public void Equals_WithDifferentTitleValue_ShouldReturnFalse()
    {
        var title1 = new WhiteboardTitle("Test Title");
        var title2 = new WhiteboardTitle("Different Title");
        
        Assert.That(title1, Is.Not.EqualTo(title2));       
    }  
}