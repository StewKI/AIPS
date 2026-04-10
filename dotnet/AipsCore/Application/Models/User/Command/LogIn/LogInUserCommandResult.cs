using AipsCore.Application.Abstract.Command;

namespace AipsCore.Application.Models.User.Command.LogIn;

public sealed record LogInUserCommandResult(string AccessToken, string RefreshToken) : ICommandResult;