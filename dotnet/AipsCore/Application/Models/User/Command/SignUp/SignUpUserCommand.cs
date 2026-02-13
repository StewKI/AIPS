using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Application.Models.User.Command.SignUp;

public record SignUpUserCommand(
    string Username,
    string Email,
    string Password) : ICommand<UserId>;