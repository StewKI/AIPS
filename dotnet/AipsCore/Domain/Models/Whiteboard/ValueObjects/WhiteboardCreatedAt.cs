using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;
using AipsCore.Domain.Common.Validation.Rules;

namespace AipsCore.Domain.Models.Whiteboard.ValueObjects;

public record WhiteboardCreatedAt : AbstractValueObject
{
    public DateTime CreatedAtValue { get; init; }

    public WhiteboardCreatedAt(DateTime CreatedAtValue)
    {
        this.CreatedAtValue = CreatedAtValue;
        Validate();
    }
    
    public override ICollection<IRule> GetValidationRules()
    {
        return [
            new DateInPastRule(CreatedAtValue)
        ];
    }
}