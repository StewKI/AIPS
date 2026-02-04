using AipsCore.Domain.Common.ValueObjects;

namespace AipsCore.Domain.Models.User.ValueObjects;

public record UserId(string IdValue) : DomainId(IdValue)
{
    public static UserId Any() => new(Guid.NewGuid().ToString());
}