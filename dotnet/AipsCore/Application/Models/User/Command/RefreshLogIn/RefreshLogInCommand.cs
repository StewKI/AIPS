using AipsCore.Application.Abstract.Command;

namespace AipsCore.Application.Models.User.Command.RefreshLogIn;

public sealed record RefreshLogInCommand(string RefreshToken) : ICommand<RefreshLogInCommandResult>;