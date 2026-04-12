using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Common.Authentication.Models;
using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Application.Models.User.Command.LogOut;

public record LogOutCommandHandlerContext(
    UserId UserId,
    RefreshToken RefreshToken
    ) : ICommandHandlerContext;