using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Common.Authentication.Dtos;

namespace AipsCore.Application.Models.User.Command.RefreshLogIn;

public record RefreshLogInCommand(string RefreshToken) : ICommand<LogInUserResultDto>;