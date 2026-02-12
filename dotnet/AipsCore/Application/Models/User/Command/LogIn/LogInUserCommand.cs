using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Authentication;

namespace AipsCore.Application.Models.User.Command.LogIn;

public record LogInUserCommand(string Email, string Password) : ICommand<Token>;