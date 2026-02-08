using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;
using AipsCore.Domain.Common.Validation.Rules;

namespace AipsCore.Domain.Models.Shape.ValueObjects;

public record Thickness : AbstractValueObject
{
    private const int MaxThickness = 8;
    private const int MinThickness = 1;
    
    private readonly int _value;

    public Thickness(int value)
    {
        _value = value;
    }
    
    protected override ICollection<IRule> GetValidationRules()
    {
        return
        [
            new MinValueRule<int>(_value, MinThickness),
            new MaxValueRule<int>(_value, MaxThickness),
        ];
    }
}