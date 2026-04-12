using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;
using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Domain.Models.Whiteboard.Options;
using AipsCore.Domain.Models.Whiteboard.Validation.Rules;

namespace AipsCore.Domain.Models.Whiteboard.ValueObjects;

public record WhiteboardTitle : AbstractValueObject
{
    public string TitleValue { get; init; }

    public WhiteboardTitle(string TitleValue)
    {
        this.TitleValue = TitleValue;
        ValidateObject();
    }   

    public override ICollection<IRule> GetValidationRules()
    {
        return [
            new MaxLengthRule(TitleValue, WhiteboardOptionsDefaults.MaxTitleLength),
            new MinLengthRule(TitleValue, WhiteboardOptionsDefaults.MinTitleLength),
            new WhiteboardTitleCharsetRule(TitleValue)
        ];
    }
}