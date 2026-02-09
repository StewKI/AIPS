using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;
using AipsCore.Domain.Common.Validation.Rules;

namespace AipsCore.Domain.Models.Shape.ValueObjects;

public record Thickness : AbstractValueObject
{
    public int Value { get; }
    private const int MaxThickness = 8;
    private const int MinThickness = 1;
    

    public Thickness(int value)
    {
        Value = value;
        Validate();
    }
    
    protected override ICollection<IRule> GetValidationRules()
    {
        return
        [
            new MinValueRule<int>(Value, MinThickness),
            new MaxValueRule<int>(Value, MaxThickness),
        ];
    }
}