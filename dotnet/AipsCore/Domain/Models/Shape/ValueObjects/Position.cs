using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;

namespace AipsCore.Domain.Models.Shape.ValueObjects;

public record Position : AbstractValueObject
{
    public int X { get; set; }
    public int Y { get; set; }

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
    
    public static Position operator -(Position position, Position otherPosition)
    {
        return new Position(position.X - otherPosition.X, position.Y - otherPosition.Y);
    }
    
    public static Position operator +(Position position, Position otherPosition)
    {
        return new Position(position.X + otherPosition.X, position.Y + otherPosition.Y);
    }
};