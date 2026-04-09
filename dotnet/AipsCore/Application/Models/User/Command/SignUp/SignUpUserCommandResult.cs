using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Application.Models.User.Command.SignUp;

public record SignUpUserCommandResult(string AccessToken, string RefreshToken) : ICommandResult;