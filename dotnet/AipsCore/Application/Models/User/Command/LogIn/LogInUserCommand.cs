using AipsCore.Application.Abstract.Command;

namespace AipsCore.Application.Models.User.Command.LogIn;

public sealed record LogInUserCommand(string Email, string Password) : ICommand<LogInUserCommandResult>;