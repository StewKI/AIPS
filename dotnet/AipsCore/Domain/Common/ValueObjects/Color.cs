using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;
using AipsCore.Domain.Common.Validation.Rules;

namespace AipsCore.Domain.Common.ValueObjects;

public record Color : AbstractValueObject
{
    public string Value { get; }

    public Color(string value)
    {
        Value = value;
    }
    
    protected override ICollection<IRule> GetValidationRules()
    {
        return
        [
            new ColorFormatRule(Value)
        ];
    }
}