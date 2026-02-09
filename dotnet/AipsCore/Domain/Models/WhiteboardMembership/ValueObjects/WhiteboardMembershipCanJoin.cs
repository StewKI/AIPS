using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;

namespace AipsCore.Domain.Models.WhiteboardMembership.ValueObjects;

public record WhiteboardMembershipCanJoin : AbstractValueObject
{
    public bool CanJoinValue { get; init; }

    public WhiteboardMembershipCanJoin(bool CanJoinValue)
    {
        this.CanJoinValue = CanJoinValue;
        Validate();
    }
    
    protected override ICollection<IRule> GetValidationRules()
    {
        return [];
    }
}