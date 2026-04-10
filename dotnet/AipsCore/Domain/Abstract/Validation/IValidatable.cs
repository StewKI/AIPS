using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Common.Validation;

namespace AipsCore.Domain.Abstract.Validation;

public interface  IValidatable
{
    ICollection<IRule> GetValidationRules();

    void Validate()
    {
        var rules = GetValidationRules();
        var validator = new Validator(rules, ValidatableObjectName);
        
        validator.Validate();

        if (!validator.Success)
        {
            throw validator.GetValidationException();
        }
    }
    
    string ValidatableObjectName => GetType().Name;
}

public interface IValidatable<in T1>
{
    ICollection<IRule> GetValidationRules(T1 arg1);

    void Validate(T1 arg1)
    {
        var rules = GetValidationRules(arg1);
        var validator = new Validator(rules, ValidatableObjectName);
        
        validator.Validate();

        if (!validator.Success)
        {
            throw validator.GetValidationException();
        }
    }
    
    string ValidatableObjectName => GetType().Name;
}

public interface IValidatable<in T1, in T2>
{
    ICollection<IRule> GetValidationRules(T1 arg1, T2 arg2);

    void Validate(T1 arg1, T2 arg2)
    {
        var rules = GetValidationRules(arg1, arg2);
        var validator = new Validator(rules, ValidatableObjectName);
        
        validator.Validate();

        if (!validator.Success)
        {
            throw validator.GetValidationException();
        }
    }
    
    string ValidatableObjectName => GetType().Name;
}