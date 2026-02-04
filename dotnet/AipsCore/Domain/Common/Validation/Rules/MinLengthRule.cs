using AipsCore.Domain.Abstract;
using AipsCore.Domain.Abstract.Rule;

namespace AipsCore.Domain.Common.Validation.Rules;

public class MinLengthRule : AbstractRule
{
    private readonly string _stringValue;
    private readonly int _minimumLentgh;
    protected override string ErrorCode => "minimum_length";
    protected override string ErrorMessage 
        => $"Length of '{ValueObjectName}' must be at least {_minimumLentgh} characters";

    public MinLengthRule(string stringValue, int minimumLentgh)
    {
        _stringValue = stringValue;
        _minimumLentgh = minimumLentgh;
    }
    
    public override bool Validate()
    {
        return _stringValue.Length >= _minimumLentgh;
    }
}