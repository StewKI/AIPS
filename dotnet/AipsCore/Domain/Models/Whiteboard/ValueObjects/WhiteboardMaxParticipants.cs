using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;
using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Domain.Models.Whiteboard.Options;

namespace AipsCore.Domain.Models.Whiteboard.ValueObjects;

public record WhiteboardMaxParticipants : AbstractValueObject
{
    public int MaxParticipantsValue { get; init; }

    public WhiteboardMaxParticipants(int MaxParticipantsValue)
    {
        this.MaxParticipantsValue = MaxParticipantsValue;
        ValidateObject();
    }
    public override ICollection<IRule> GetValidationRules()
    {
        return [
            new MinValueRule<int>(MaxParticipantsValue, WhiteboardOptionsDefaults.MinMaxParticipants),
            new MaxValueRule<int>(MaxParticipantsValue, WhiteboardOptionsDefaults.MaxMaxParticipants)
        ];
    }
}