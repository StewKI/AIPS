using AipsCore.Domain.Models.User;
using AipsCore.Domain.Models.User.External;

namespace AipsCore.Application.Common.Authentication;

public record LoginResult(User User, IList<UserRole> Roles);