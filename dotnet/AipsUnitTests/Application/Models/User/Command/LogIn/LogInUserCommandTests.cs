using AipsCore.Application.Abstract.UserContext;
using AipsCore.Application.Common.Authentication;
using AipsCore.Application.Models.User.Command.LogIn;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.User.External;
using AipsCore.Domain.Models.User.Validation;
using Moq;

using UserModel = AipsCore.Domain.Models.User.User;

namespace AipsNUnitTests.Application.Models.User.Command.LogIn;

[TestFixture]
public class LogInUserCommandTests
{
    private Mock<ITokenProvider> _tokenProviderMock;
    private Mock<IRefreshTokenManager> _refreshTokenManagerMock;
    private Mock<IAuthService> _authServiceMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private LogInUserCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _tokenProviderMock = new Mock<ITokenProvider>();
        _refreshTokenManagerMock = new Mock<IRefreshTokenManager>();
        _authServiceMock = new Mock<IAuthService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new LogInUserCommandHandler(
            _tokenProviderMock.Object,
            _refreshTokenManagerMock.Object,
            _authServiceMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Test]
    public async Task Handle_ValidCredentials_ShouldReturnTokensAndPersistRefreshToken()
    {
        var command = new LogInUserCommand("test@example.com", "Password123!");
        var user = UserModel.Create("test@example.com", "testuser");
        var roles = new List<UserRole> { UserRole.User };
        var loginResult = new LoginResult(user, roles);
        
        _authServiceMock
            .Setup(x => x.LoginWithEmailAndPasswordAsync(command.Email, command.Password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(loginResult);
        
        _tokenProviderMock
            .Setup(x => x.GenerateAccessToken(user, roles))
            .Returns("access_token");
        
        _tokenProviderMock
            .Setup(x => x.GenerateRefreshToken())
            .Returns("refresh_token");
        
        var result = await _handler.Handle(command);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.AccessToken, Is.EqualTo("access_token"));
            Assert.That(result.RefreshToken, Is.EqualTo("refresh_token"));
        });
        
        _refreshTokenManagerMock.Verify(x => x.AddAsync("refresh_token", user.Id, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Test]
    public void Handle_InvalidCredentials_ShouldThrowValidationException()
    {
        var command = new LogInUserCommand("wrong@example.com", "WrongPassword");
        
        _authServiceMock
            .Setup(x => x.LoginWithEmailAndPasswordAsync(command.Email, command.Password, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ValidationException(UserErrors.InvalidCredentials()));

        var ex = Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(command));
        Assert.That(ex.ValidationErrors.First().Code, Is.EqualTo("invalid_credentials"));
        
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}