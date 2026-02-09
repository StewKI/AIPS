using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Application.Models.User.Command.CreateUser;

public record CreateUserCommand(
    string Username,
    string Email,
    DateTime CreatedAt,
    DateTime DeletedAt)
    : ICommand<UserId>;