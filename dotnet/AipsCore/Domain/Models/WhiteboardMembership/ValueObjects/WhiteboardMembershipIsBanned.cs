using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;

namespace AipsCore.Domain.Models.WhiteboardMembership.ValueObjects;

public record WhiteboardMembershipIsBanned : AbstractValueObject
{
    public bool IsBannedValue { get; init; }

    public WhiteboardMembershipIsBanned(bool IsBannedValue)
    {
        this.IsBannedValue = IsBannedValue;
        Validate();
    }
    
    protected override ICollection<IRule> GetValidationRules()
    {
        return [];
    }
}