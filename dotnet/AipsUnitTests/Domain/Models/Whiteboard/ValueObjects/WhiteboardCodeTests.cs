using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Domain.Models.Whiteboard.Validation.Rules;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsTestsUtility;

namespace AipsNUnitTests.Domain.Models.Whiteboard.ValueObjects;

[TestFixture]
public class WhiteboardCodeTests
{
    [TestCase("12345678")]
    public void Constructor_WithValidCode_ShouldCreateCode(string validCode)
    {
        Assert.DoesNotThrow(() => _ = new WhiteboardCode(validCode));
    }
    
    [TestCase("")]
    [TestCase("1")]
    [TestCase("123456789")]
    public void Constructor_WithInvalidCode_ShouldBreakWhiteboardCodeLengthRule(string invalidCode)
    {
        var exception = Assert.Throws<ValidationException>(() => _ = new WhiteboardCode(invalidCode));
        
        AssertUtility.AssertHasBrokenExactRule<ExactLength>(exception);
    }

    [TestCase("ABCDEFGH")]
    [TestCase("?.,=!+#_")]
    public void Constructor_WithInvalidCodeCharset_ShouldBreakWhiteboardCodeCharsetRule(string invalidCode)
    {
        var exception = Assert.Throws<ValidationException>(() => _ = new WhiteboardCode(invalidCode));
        
        AssertUtility.AssertHasBrokenExactRule<WhiteboardCodeCharsetRule>(exception);
    }
    
    [Test]
    public void Equals_WithSameCodeValue_ShouldReturnTrue()
    {
        var code1 = new WhiteboardCode("12345678");
        var code2 = new WhiteboardCode("12345678");
        
        Assert.That(code1, Is.EqualTo(code2));
    }
    
    [Test]
    public void Equals_WithDifferentCodeValue_ShouldReturnFalse()
    {
        var code1 = new WhiteboardCode("12345678");
        var code2 = new WhiteboardCode("98765432");
        
        Assert.That(code1, Is.Not.EqualTo(code2));       
    }  
}