using System.Numerics;
using AipsCore.Domain.Abstract.Rule;

namespace AipsCore.Domain.Common.Validation.Rules;

public class MaxValueRule<T> : AbstractRule, IRuleMetadata
    where T : INumber<T>
{
    private readonly T _value;
    private readonly T _maximum;

    public MaxValueRule(T value, T maximum)
    {
        _value = value;
        _maximum = maximum;
    }
    
    protected override string ErrorCode => ErrorCodeString;
    protected override string ErrorMessage => $"Value of '{ValueObjectName}' should be at most {_maximum}";
    public override bool Validate()
    {
        return _value <= _maximum;
    }

    public static string ErrorCodeString => "max_value";
}