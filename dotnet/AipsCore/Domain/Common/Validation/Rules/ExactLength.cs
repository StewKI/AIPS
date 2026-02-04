using AipsCore.Domain.Abstract;
using AipsCore.Domain.Abstract.Rule;

namespace AipsCore.Domain.Common.Validation.Rules;

public class ExactLength : AbstractRule
{
    private readonly string _stringValue;
    private readonly int _exactLentgh;
    protected override string ErrorCode => "exact_length";
    protected override string ErrorMessage 
        => $"Length of '{ValueObjectName}' must be {_exactLentgh} characters";
    
    public ExactLength(string stringValue, int exactLentgh)
    {
        _stringValue = stringValue;
        _exactLentgh = exactLentgh;
    }
    
    public override bool Validate()
    {
        return _stringValue.Length == _exactLentgh;
    }
}