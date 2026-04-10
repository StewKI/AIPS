using AipsCore.Application.Abstract.Command;

namespace AipsCore.Application.Models.User.Command.SignUp;

public sealed record SignUpUserCommand(
    string Username,
    string Email,
    string Password) 
    : ICommand<SignUpUserCommandResult>;