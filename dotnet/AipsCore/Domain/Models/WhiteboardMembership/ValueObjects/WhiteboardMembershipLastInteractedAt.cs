using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;
using AipsCore.Domain.Common.Validation.Rules;

namespace AipsCore.Domain.Models.WhiteboardMembership.ValueObjects;

public record WhiteboardMembershipLastInteractedAt : AbstractValueObject
{
    public DateTime LastInteractedAtValue { get; init; }

    public WhiteboardMembershipLastInteractedAt(DateTime LastInteractedAtValue)
    {
        this.LastInteractedAtValue = LastInteractedAtValue;
        Validate();
    }
    
    protected override ICollection<IRule> GetValidationRules()
    {
        return [
            new DateInPastRule(LastInteractedAtValue)
        ];
    }
}