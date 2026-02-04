using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Common.Validation;

namespace AipsCore.Domain.Abstract.ValueObject;

public abstract record AbstractValueObject
{
    protected abstract ICollection<IRule> GetValidationRules();

    protected void Validate()
    {
        var rules = GetValidationRules();
        var validator = new Validator(rules, ValueObjectName);
        
        validator.Validate();

        if (!validator.Success)
        {
            throw validator.GetValidationException();
        }
    }

    private string ValueObjectName => this.GetType().Name;
}