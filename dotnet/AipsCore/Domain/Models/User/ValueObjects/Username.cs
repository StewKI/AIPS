using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;
using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Domain.Models.User.Options;
using AipsCore.Domain.Models.User.Validation;
using AipsCore.Domain.Models.User.Validation.Rules;

namespace AipsCore.Domain.Models.User.ValueObjects;

public record Username : AbstractValueObject
{
    public string UsernameValue { get; init; }
    
    public Username(string UsernameValue)
    {
        this.UsernameValue = UsernameValue;
        ValidateObject();
    }

    public override ICollection<IRule> GetValidationRules()
    {
        return
        [
            new MinLengthRule(UsernameValue, UserOptionsDefaults.UsernameMinimumLength),
            new MaxLengthRule(UsernameValue, UserOptionsDefaults.UsernameMaximumLength),
            new UsernameCharsetRule(UsernameValue)
        ];
    }
}