using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Application.Models.User.Command.SignUp;

public sealed record SignUpUserCommandResult(string AccessToken, string RefreshToken) : ICommandResult;