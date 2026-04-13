using System.Numerics;
using AipsCore.Domain.Abstract.Rule;

namespace AipsCore.Domain.Common.Validation.Rules;

public class MinValueRule<T> : AbstractRule, IRuleMetadata
    where T : INumber<T>
{
    private readonly T _value;
    private readonly T _minimum;

    public MinValueRule(T value, T minimum)
    {
        _value = value;
        _minimum = minimum;
    }

    protected override string ErrorCode => ErrorCodeString;
    protected override string ErrorMessage => $"Value of '{ValueObjectName}' should be at least {_minimum}";
    public override bool Validate()
    {
        return _value >= _minimum;
    }

    public static string ErrorCodeString => "min_value";
}