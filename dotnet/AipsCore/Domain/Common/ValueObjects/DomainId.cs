using AipsCore.Domain.Abstract;
using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;
using AipsCore.Domain.Common.Validation;

namespace AipsCore.Domain.Common.ValueObjects;

public record DomainId : AbstractValueObject
{
    public string IdValue { get; init; }

    public DomainId(string IdValue)
    {
        this.IdValue = IdValue;
        Validate();
    }

    protected override ICollection<IRule> GetValidationRules()
    {
        return [];
    }
}