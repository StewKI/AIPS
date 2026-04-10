using AipsCore.Application.Abstract.Command;

namespace AipsCore.Application.Models.User.Command.LogOut;

public sealed record LogOutCommand(string RefreshToken) : ICommand;