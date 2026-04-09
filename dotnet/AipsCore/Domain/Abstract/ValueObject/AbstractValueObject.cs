using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.Validation;

namespace AipsCore.Domain.Abstract.ValueObject;

public abstract record AbstractValueObject : IValidatable
{
    public abstract ICollection<IRule> GetValidationRules();

    protected void Validate() => ((IValidatable)this).Validate();
}