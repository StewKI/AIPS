using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;
using AipsCore.Domain.Common.Validation.Rules;

namespace AipsCore.Domain.Models.User.ValueObjects;

public record UserDeletedAt : AbstractValueObject
{
    public DateTime? DeletedAtValue { get; private set; }
    
    public UserDeletedAt(DateTime? DeletedAtValue)
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