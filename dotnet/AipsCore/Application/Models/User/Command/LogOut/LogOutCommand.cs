using AipsCore.Application.Abstract.Command;

namespace AipsCore.Application.Models.User.Command.LogOut;

public record LogOutCommand(string RefreshToken) : ICommand;