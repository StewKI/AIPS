using AipsCore.Application.Abstract.Command;

namespace AipsCore.Application.Models.User.Command.SignUp;

public record SignUpUserCommand(
    string Username,
    string Email,
    string Password) 
    : ICommand<SignUpUserCommandResult>;