using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;

namespace AipsCore.Domain.Models.Shape.Sub.TextShape.ValueObjects;

public record TextShapeValue: AbstractValueObject
{
    public string Text { get; }

    public TextShapeValue(string text)
    {
        Text = text;
    }
    
    protected override ICollection<IRule> GetValidationRules()
    {
        return [];
    }
}