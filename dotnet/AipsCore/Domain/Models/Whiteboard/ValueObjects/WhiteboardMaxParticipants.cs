using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;
using AipsCore.Domain.Common.Validation.Rules;

namespace AipsCore.Domain.Models.Whiteboard.ValueObjects;

public record WhiteboardMaxParticipants : AbstractValueObject
{
    private const int MinMaxParticipants = 1;
    private const int MaxMaxParticipants = 100;
    public int MaxParticipantsValue { get; init; }

    public WhiteboardMaxParticipants(int MaxParticipantsValue)
    {
        this.MaxParticipantsValue = MaxParticipantsValue;
        Validate();
    }
    protected override ICollection<IRule> GetValidationRules()
    {
        return [
            new MinValueRule<int>(MaxParticipantsValue, MinMaxParticipants),
            new MaxValueRule<int>(MaxParticipantsValue, MaxMaxParticipants)
        ];
    }
}