using System.Numerics;
using AipsCore.Domain.Abstract.Rule;

namespace AipsCore.Domain.Common.Validation.Rules;

public class MinValueRule<T>: AbstractRule where T : INumber<T>
{
    private readonly T _value;
    private readonly T _minimum;

    public MinValueRule(T value, T minimum)
    {
        _value = value;
        _minimum = minimum;
    }
    
    protected override string ErrorCode => "min_value";
    protected override string ErrorMessage => $"Value of '{ValueObjectName}' should be at least {_minimum}";
    public override bool Validate()
    {
        return _value >= _minimum;
    }
}