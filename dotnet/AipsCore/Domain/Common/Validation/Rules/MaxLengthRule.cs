using AipsCore.Domain.Abstract;
using AipsCore.Domain.Abstract.Rule;

namespace AipsCore.Domain.Common.Validation.Rules;

public class MaxLengthRule : AbstractRule
{
    private readonly string _stringValue;
    private readonly int _maximumLentgh;
    protected override string ErrorCode => "minimum_length";
    protected override string ErrorMessage 
        => $"Length of '{ValueObjectName}' must be at most {_maximumLentgh} characters";

    public MaxLengthRule(string stringValue, int maximumLentgh)
    {
        _stringValue = stringValue;
        _maximumLentgh = maximumLentgh;
    }
    
    public override bool Validate()
    {
        return _stringValue.Length <= _maximumLentgh;
    }
}