using AipsCore.Domain.Common.Validation;

namespace AipsCore.Domain.Abstract.Rule;

public interface IRule
{
    ValidationError GetError();
    bool Validate();
    string ValueObjectName { set; }
}