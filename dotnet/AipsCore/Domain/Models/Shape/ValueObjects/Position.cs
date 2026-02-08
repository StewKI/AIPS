using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;

namespace AipsCore.Domain.Models.Shape.ValueObjects;

public record Position : AbstractValueObject
{
    public int X { get; }
    public int Y { get; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    protected override ICollection<IRule> GetValidationRules()
    {
        return [
            
        ];
    }
};