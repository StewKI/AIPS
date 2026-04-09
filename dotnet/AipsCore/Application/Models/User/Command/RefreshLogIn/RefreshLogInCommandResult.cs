using AipsCore.Application.Abstract.Command;

namespace AipsCore.Application.Models.User.Command.RefreshLogIn;

public record RefreshLogInCommandResult(string AccessToken, string RefreshToken) : ICommandResult;