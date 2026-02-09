using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;
using AipsCore.Domain.Common.Validation.Rules;

namespace AipsCore.Domain.Models.User.ValueObjects;

public record UserCreatedAt : AbstractValueObject
{
    public DateTime CreatedAtValue { get; private set; }

    public UserCreatedAt(DateTime CreatedAtValue)
    {
        this.CreatedAtValue = CreatedAtValue;
            Validate();
    }
    
    protected override ICollection<IRule> GetValidationRules()
    {
        return [
            new DateInPastRule(CreatedAtValue)
        ];
    }
}