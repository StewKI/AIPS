using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;
using AipsCore.Domain.Common.Validation.Rules;

namespace AipsCore.Domain.Models.Shape.Sub.TextShape.ValueObjects;

public record TextShapeSize : AbstractValueObject
{
    public const int MaxTextShapeSize = 72;
    public const int MinTextShapeSize = 8;
    
    public int Size { get; }

    public TextShapeSize(int size)
    {
        Size = size;
    }
    
    protected override ICollection<IRule> GetValidationRules()
    {
        return
        [
            new MaxValueRule<int>(Size, MaxTextShapeSize),
            new MinValueRule<int>(Size, MinTextShapeSize)
        ];
    }
}