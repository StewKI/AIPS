using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Domain.Models.User.Validation.Rules;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsTestsUtility;

namespace AipsNUnitTests.Domain.Models.User.ValueObjects;

[TestFixture]
public class UsernameTests
{
    [TestCase("TestUser")]
    [TestCase("Test_User_123")]
    [TestCase("TestUserTestUserTest")]
    public void Constructor_WithValidUsername_ShouldCreateUsername(string validUsername)
    {
        Assert.DoesNotThrow(() => _ = new Username(validUsername));
    }
    
    [TestCase("")]
    [TestCase("Test")]
    public void Constructor_WithUsernameTooShort_ShouldBreakUsernameMinLengthRule(string usernameTooShort)
    {
        var exception = Assert.Throws<ValidationException>(() => _ = new Username(usernameTooShort));
        
        AssertUtility.AssertHasBrokenExactRule<MinLengthRule>(exception);
    }
    
    [TestCase("TestUserTestUserTestUserTestUserTestUserTestUser")]
    public void Constructor_WithUsernameTooLong_ShouldBreakUsernameMaxLengthRule(string usernameTooLong)
    {
        var exception = Assert.Throws<ValidationException>(() => _ = new Username(usernameTooLong));
        
        AssertUtility.AssertHasBrokenExactRule<MaxLengthRule>(exception);
    }
    
    [TestCase("TestUser!")]
    [TestCase("Test User")]
    [TestCase("TestUser@")]
    [TestCase("TestUser#")]
    public void Constructor_WithInvalidCharacters_ShouldBreakUsernameCharsetRule(string invalidCharsetUsername)
    {
        var exception = Assert.Throws<ValidationException>(() => _ = new Username(invalidCharsetUsername));
        
        AssertUtility.AssertHasBrokenExactRule<UsernameCharsetRule>(exception);
    }

    [Test]
    public void Equals_WithSameUsernameValue_ShouldReturnTrue()
    {
        var username1 = new Username("TestUser");
        var username2 = new Username("TestUser");
        
        Assert.That(username1, Is.EqualTo(username2));       
    }
    
    [Test]
    public void Equals_WithDifferentUsernameValue_ShouldReturnFalse()
    {
        var username1 = new Username("TestUser");
        var username2 = new Username("DifferentUser");
        
        Assert.That(username1, Is.Not.EqualTo(username2));       
    }   
}