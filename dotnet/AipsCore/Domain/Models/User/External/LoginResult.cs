namespace AipsCore.Domain.Models.User.External;

public record LoginResult(User User, IList<string> Roles);