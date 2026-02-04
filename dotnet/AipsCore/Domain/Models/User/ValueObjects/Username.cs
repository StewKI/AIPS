using AipsCore.Domain.Abstract;
using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;
using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Domain.Common.ValueObjects;
using AipsCore.Domain.Models.User.Validation;

namespace AipsCore.Domain.Models.User.ValueObjects;

public record Username : AbstractValueObject
{
    public string UsernameValue { get; init; }
    
    public Username(string UsernameValue)
    {
        this.UsernameValue = UsernameValue;
        Validate();
    }

    private const int MinimumLength = 8;
    private const int MaximumLength = 20;

    protected override ICollection<IRule> GetValidationRules()
    {
        return
        [
            new MinLengthRule(UsernameValue, MinimumLength),
            new MaxLengthRule(UsernameValue, MaximumLength),
            new UsernameCharsetRule(UsernameValue)
        ];
    }
}