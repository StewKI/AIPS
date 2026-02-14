using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Common.Authentication;
using AipsCore.Application.Common.Authentication.Dtos;

namespace AipsCore.Application.Models.User.Command.LogIn;

public record LogInUserCommand(string Email, string Password) : ICommand<LogInUserResultDto>;