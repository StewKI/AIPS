using AipsCore.Domain.Common.Validation;

namespace AipsCore.Domain.Abstract.Rule;

public abstract class AbstractRule : IRule
{
    protected abstract string ErrorCode { get; }
    protected abstract string ErrorMessage { get; }

    public string ValueObjectName { protected get; set; } = "Unknown";
    
    public ValidationError GetError()
    {
        return new ValidationError(ErrorCode, ErrorMessage);
    }

    public abstract bool Validate();
}