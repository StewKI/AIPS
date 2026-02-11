using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;

namespace AipsCore.Domain.Models.WhiteboardMembership.ValueObjects;

public record WhiteboardMembershipEditingEnabled : AbstractValueObject
{
    public bool EditingEnabledValue { get; init; }

    public WhiteboardMembershipEditingEnabled(bool EditingEnabledValue)
    {
        this.EditingEnabledValue = EditingEnabledValue;
        Validate();
    }
    
    protected override ICollection<IRule> GetValidationRules()
    {
        return [];
    }

    public static WhiteboardMembershipEditingEnabled Enabled
        => new WhiteboardMembershipEditingEnabled(true);
    
    public static WhiteboardMembershipEditingEnabled Disabled
        => new WhiteboardMembershipEditingEnabled(false);
}