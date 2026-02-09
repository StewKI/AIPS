using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;
using AipsCore.Domain.Common.Validation.Rules;

namespace AipsCore.Domain.Models.Whiteboard.ValueObjects;

public record WhiteboardDeletedAt : AbstractValueObject
{
    public DateTime? DeletedAtValue { get; init; }

    public WhiteboardDeletedAt(DateTime? DeletedAtValue)
    {
        this.DeletedAtValue = DeletedAtValue;
        Validate();
    }
    
    protected override ICollection<IRule> GetValidationRules()
    {
        return [
            new DateInPastRule(DeletedAtValue)
        ];
    }
}