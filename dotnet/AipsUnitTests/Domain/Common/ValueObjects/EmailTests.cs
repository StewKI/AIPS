using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Domain.Common.ValueObjects;
using AipsTestsUtility;

namespace AipsNUnitTests.Domain.Common.ValueObjects;

[TestFixture]
public class EmailTests
{
    [TestCase("test@test.test")]
    public void Constructor_WithValidEmail_ShouldCreateEmail(string validEmail)
    {
        Assert.DoesNotThrow(() => _ = new Email(validEmail));
    }

    [TestCase("")]
    [TestCase("invalid-email")]
    [TestCase("test@")]
    [TestCase("@example.com")]
    //[TestCase("test@example")]
    public void Constructor_WithInvalidEmail_ShouldBreakEmailRule(string invalidEmail)
    {
        var exception = Assert.Throws<ValidationException>(() => _ = new Email(invalidEmail));
        
        AssertUtility.AssertHasBrokenExactRule<EmailRule>(exception);
    }

    [Test]
    public void Equals_WithSameEmailValue_ShouldReturnTrue()
    {
        var email1 = new Email("test@example.com");
        var email2 = new Email("test@example.com");

        Assert.That(email1, Is.EqualTo(email2));
    }

    [Test]   
    public void Equals_WithDifferentEmailValue_ShouldReturnFalse()
    {
        var email1 = new Email("example@test.com");
        var email2 = new Email("test@example.com");

        Assert.That(email1, Is.Not.EqualTo(email2));
    }
}