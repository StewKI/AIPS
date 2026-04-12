using System.Diagnostics.CodeAnalysis;
using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Common.Authentication;
using AipsCore.Application.Models.User.Command.LogIn;
using AipsCore.Domain.Common.Validation;
using AipsCore.Infrastructure.Persistence.Db;
using AipsIntegrationTests.Abstract;
using Microsoft.EntityFrameworkCore;
using UserModel = AipsCore.Domain.Models.User.User;

namespace AipsIntegrationTests.Application.Models.User.Command.LogIn;

[TestFixture]
[SuppressMessage("NUnit.Framework", "NUnit1032")]
public class LogInUserCommandTests : IntegrationTestBase
{
    private IAuthService _authService = null!;
    private AipsDbContext _dbContext = null!;
    
    private ICommandHandler<LogInUserCommand, LogInUserCommandResult> _handler = null!;

    private string _email = null!;
    private string _password = null!;

    [SetUp]
    public async Task SetUp()
    {
        _authService = GetService<IAuthService>();
        _dbContext = GetService<AipsDbContext>();
        _handler = GetService<ICommandHandler<LogInUserCommand, LogInUserCommandResult>>();

        _email = "integration-test@example.com";
        _password = "SecurePassword123!";

        var user = UserModel.Create(_email, "TestUser");

        await _authService.SignUpWithPasswordAsync(user, _password);
    }
    
    [Test]
    public async Task Handle_ValidCredentials_ShouldReturnTokensAndPersistRefreshToken()
    {
        var command = new LogInUserCommand(_email, _password);
        var handler = GetService<ICommandHandler<LogInUserCommand, LogInUserCommandResult>>();
        
        var result = await handler.Handle(command);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.AccessToken, Is.Not.Null.And.Not.Empty);
            Assert.That(result.RefreshToken, Is.Not.Null.And.Not.Empty);
        });
        
        var savedToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == result.RefreshToken);
        
        Assert.That(savedToken, Is.Not.Null);
        
        var dbUser = await _dbContext.Users.FirstAsync(u => u.Email == _email);
        Assert.That(savedToken.UserId, Is.EqualTo(dbUser.Id));
    }

    [Test]
    public void Handle_InvalidPassword_ShouldThrowValidationException()
    {
        var incorrectPassword = "IncorrectPassword!";
        
        var command = new LogInUserCommand(_email, incorrectPassword);
        
        Assert.ThrowsAsync<ValidationException>(async () => _ = await _handler.Handle(command));
    }
    
    [Test]
    public void Handle_InvalidEmail_ShouldThrowValidationException()
    {
        var incorrectEmail = "invalid-email@example.com";
        
        var command = new LogInUserCommand(incorrectEmail, _password);
        
        Assert.ThrowsAsync<ValidationException>(async () => _ = await _handler.Handle(command));
    }
}