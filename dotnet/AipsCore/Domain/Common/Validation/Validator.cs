using AipsCore.Domain.Abstract;
using AipsCore.Domain.Abstract.Rule;

namespace AipsCore.Domain.Common.Validation;

public class Validator
{
    private readonly ICollection<IRule> _validationRules;
    private readonly string _valueObjectName;

    public Validator(ICollection<IRule> validationRules, string valueObjectName)
    {
        _validationRules = validationRules;
        _valueObjectName = valueObjectName;
    }
    
    public bool Success { get; private set; } = false;

    private ValidationException? _validationException = null;
    
    public ValidationException GetValidationException()
    {
        if (_validationException is null)
        {
            return new ValidationException([]);
        }
        return _validationException;
    }

    public void Validate()
    {
        List<ValidationError> errors = [];
        
        foreach (var validationRule in _validationRules)
        {
            validationRule.ValueObjectName = _valueObjectName;
            
            if (!validationRule.Validate())
            {
                errors.Add(validationRule.GetError());
            }
        }

        if (errors.Any())
        {
            Success = false;
            _validationException = new ValidationException(errors.ToArray());
        }
        else
        {
            Success = true;
            _validationException = null;
        }
    }
}