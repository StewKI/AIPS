using System.Diagnostics.CodeAnalysis;
using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.UserContext;
using AipsCore.Application.Common.Authentication;
using AipsCore.Application.Models.User.Command.LogIn;
using AipsCore.Application.Models.User.Command.LogOut;
using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.User.Validation.Rules;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Infrastructure.Persistence.Db;
using AipsIntegrationTests.Abstract;
using AipsTestsUtility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using UserModel = AipsCore.Domain.Models.User.User;

namespace AipsIntegrationTests.Application.Models.User.Command.LogOut;

[TestFixture]
[SuppressMessage("NUnit.Framework", "NUnit1032")]
public class LogOutCommandTests : IntegrationTestBase
{
    private Mock<IUserContext> _userContextMock = null!;
    
    private IAuthService _authService = null!;
    private AipsDbContext _dbContext = null!;
    
    private ICommandHandler<LogInUserCommand, LogInUserCommandResult> _loginHandler = null!;
    private ICommandHandler<LogOutCommand> _logoutHandler = null!;

    private const string MainUserEmail = "main@test.com";
    private const string Password = "Password123!";
    private string _tokenToRevoke = null!;

    protected override void InterceptServices(IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Transient<IUserContext>(_ => _userContextMock.Object));
    }

    [SetUp]
    public async Task SetUp()
    {
        _userContextMock = new Mock<IUserContext>();
        
        _authService = GetService<IAuthService>();
        _dbContext = GetService<AipsDbContext>();
        
        _loginHandler = GetService<ICommandHandler<LogInUserCommand, LogInUserCommandResult>>();
        _logoutHandler = GetService<ICommandHandler<LogOutCommand>>();

        var mainUser = UserModel.Create(MainUserEmail, "MainUser");
        
        _userContextMock.Setup(u => u.GetCurrentUserId()).Returns(mainUser.Id);
        
        await _authService.SignUpWithPasswordAsync(mainUser, Password);

        for (int i = 0; i < 3; i++)
        {
            var result = await _loginHandler.Handle(new LogInUserCommand(MainUserEmail, Password));
            if (i == 0)
            {
                _tokenToRevoke = result.RefreshToken;
            } 
        }

        for (int i = 1; i <= 3; i++)
        {
            var email = $"other{i}@test.com";
            var user = UserModel.Create(email, $"OtherUser{i}");
            await _authService.SignUpWithPasswordAsync(user, Password);
            
            await _loginHandler.Handle(new LogInUserCommand(email, Password));
        }
    }
    
    [Test]
    public async Task Handle_ValidRefreshToken_ShouldOnlyRemoveSpecificToken()
    {
        var initialTokenCount = await _dbContext.RefreshTokens.CountAsync();
        Assert.That(initialTokenCount, Is.EqualTo(6), "Database should be setup with 6 refresh tokens.");

        var command = new LogOutCommand(_tokenToRevoke);

        await _logoutHandler.Handle(command);

        var remainingTokens = await _dbContext.RefreshTokens.ToListAsync();
        
        Assert.Multiple(() =>
        {
            Assert.That(remainingTokens, Has.Count.EqualTo(5));

            Assert.That(remainingTokens.Any(t => t.Token == _tokenToRevoke), Is.False);

            var mainUser = _dbContext.Users.First(u => u.Email == MainUserEmail);
            var mainUserTokens = remainingTokens.Count(t => t.UserId == mainUser.Id);
            
            Assert.That(mainUserTokens, Is.EqualTo(2));
        });
    }

    [Test]
    public void Handle_InvalidUser_ShouldBreakUserCanOnlyLogOutHimselfRule()
    {
        var invalidUserId = new UserId(Guid.NewGuid().ToString());
        
        _userContextMock.Setup(u => u.GetCurrentUserId()).Returns(invalidUserId); 
        
        var command = new LogOutCommand(_tokenToRevoke);
        
        var exception = Assert.ThrowsAsync<ValidationException>(async () => await _logoutHandler.Handle(command));
        
        AssertUtility.AssertHasBrokenExactRule<UserCanOnlyLogOutHimselfRule>(exception);
    }
}