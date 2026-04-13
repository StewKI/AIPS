using UserModel = AipsCore.Domain.Models.User.User;

namespace AipsNUnitTests.Domain.Models.User;

[TestFixture]
public class UserTests
{
    [Test]
    public void Create_WithValidData_ShouldInitializeUserPropertiesCorrectly()
    {
        var email = "test@example.com";
        var username = "valid_username";

        var user = UserModel.Create(email, username);

        Assert.Multiple(() =>
        {
            Assert.That(user.Id, Is.Not.Null);
            Assert.That(user.Email.EmailValue, Is.EqualTo(email));
            Assert.That(user.Username.UsernameValue, Is.EqualTo(username));
            Assert.That(user.CreatedAt, Is.Not.Null);
            Assert.That(user.DeletedAt.DeletedAtValue, Is.Null);
        });
    }
}