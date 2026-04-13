using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;
using AipsCore.Domain.Common.Validation.Rules;

namespace AipsCore.Domain.Common.ValueObjects;

public record DeletedAt : AbstractValueObject
{
    public DateTime? DeletedAtValue { get; private set; }
    
    public DeletedAt(DateTime? DeletedAtValue)
    {
        this.DeletedAtValue = DeletedAtValue;
        ValidateObject();
    }
    
    public override ICollection<IRule> GetValidationRules()
    {
        return 
        [
            new DateInPastRule(DeletedAtValue)
        ];
    }
}