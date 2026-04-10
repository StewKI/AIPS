using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.Validation;

namespace AipsCore.Domain.Abstract.ValueObject;

public abstract record AbstractValueObject : IValidatable
{
    public virtual ICollection<IRule> GetValidationRules()
    {
        return [];
    }
    
    public void Validate() => ((IValidatable)this).Validate();
}