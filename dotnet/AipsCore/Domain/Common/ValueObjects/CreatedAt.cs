using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;
using AipsCore.Domain.Common.Validation.Rules;

namespace AipsCore.Domain.Common.ValueObjects;

public record CreatedAt : AbstractValueObject
{
    public DateTime CreatedAtValue { get; private set; }

    public CreatedAt(DateTime CreatedAtValue)
    {
        this.CreatedAtValue = CreatedAtValue;
        ValidateObject();
    }
    
    public override ICollection<IRule> GetValidationRules()
    {
        return 
        [
            new DateInPastRule(CreatedAtValue)
        ];
    }
}