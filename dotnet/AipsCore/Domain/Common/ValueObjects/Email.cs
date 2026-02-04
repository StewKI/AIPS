using AipsCore.Domain.Abstract;
using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;
using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Common.Validation.Rules;

namespace AipsCore.Domain.Common.ValueObjects;

public record Email : AbstractValueObject
{
    public string EmailValue { get; init; }

    public Email(string EmailValue)
    {
        this.EmailValue = EmailValue;
        Validate();
    }

    protected override ICollection<IRule> GetValidationRules()
    {
        return
        [
            new EmailRule(EmailValue)
        ];
    }
}