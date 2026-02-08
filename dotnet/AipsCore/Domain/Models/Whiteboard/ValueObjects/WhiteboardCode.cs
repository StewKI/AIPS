using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;
using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Domain.Models.Whiteboard.Validation;

namespace AipsCore.Domain.Models.Whiteboard.ValueObjects;

public record WhiteboardCode : AbstractValueObject
{
    public string CodeValue { get; init; }
    
    public WhiteboardCode(string CodeValue)
    {
        this.CodeValue = CodeValue;
        Validate();
    }

    private const int CodeLength = 8;

    protected override ICollection<IRule> GetValidationRules()
    {
        return
        [
            new ExactLength(CodeValue, CodeLength),
            new WhiteboardCodeCharsetRule(CodeValue)
        ];
    }
} 