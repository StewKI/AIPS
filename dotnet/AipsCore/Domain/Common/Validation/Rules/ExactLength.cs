using AipsCore.Domain.Abstract;
using AipsCore.Domain.Abstract.Rule;

namespace AipsCore.Domain.Common.Validation.Rules;

public class ExactLength : AbstractRule, IRuleMetadata
{
    private readonly string _stringValue;
    private readonly int _exactLength;
    protected override string ErrorCode => ErrorCodeString;
    protected override string ErrorMessage 
        => $"Length of '{ValueObjectName}' must be {_exactLength} characters";
    
    public ExactLength(string stringValue, int exactLength)
    {
        _stringValue = stringValue;
        _exactLength = exactLength;
    }
    
    public override bool Validate()
    {
        return _stringValue.Length == _exactLength;
    }

    public static string ErrorCodeString => "exact_length";
}