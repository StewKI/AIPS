using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;
using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Domain.Models.Shape.Sub.TextShape.Options;

namespace AipsCore.Domain.Models.Shape.Sub.TextShape.ValueObjects;

public record TextShapeSize : AbstractValueObject
{
    public int Size { get; }

    public TextShapeSize(int size)
    {
        Size = size;
        ValidateObject();
    }
    
    public override ICollection<IRule> GetValidationRules()
    {
        return
        [
            new MaxValueRule<int>(Size, TextShapeOptionsDefaults.MaxTextShapeSize),
            new MinValueRule<int>(Size, TextShapeOptionsDefaults.MinTextShapeSize)
        ];
    }
}