using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;
using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Domain.Models.Whiteboard.Validation;

namespace AipsCore.Domain.Models.Whiteboard.ValueObjects;

public record WhiteboardTitle : AbstractValueObject
{
    private const int MaxWhiteboardTitleLength = 32;
    private const int MinWhiteboardTitleLength = 3;
    
    public string TitleValue { get; init; }

    public WhiteboardTitle(string TitleValue)
    {
        this.TitleValue = TitleValue;
        Validate();
    }   

    protected override ICollection<IRule> GetValidationRules()
    {
        return [
            new MaxLengthRule(TitleValue, MaxWhiteboardTitleLength),
            new MinLengthRule(TitleValue, MinWhiteboardTitleLength),
            new WhiteboardTitleCharsetRule(TitleValue)
        ];
    }
}