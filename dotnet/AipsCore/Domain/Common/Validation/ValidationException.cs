using System.Collections.Immutable;

namespace AipsCore.Domain.Common.Validation;

public class ValidationException : Exception
{
    public ICollection<ValidationError> ValidationErrors { get; private init; }
    
    public ValidationException(ICollection<ValidationError> validationErrors)
    {
        ValidationErrors = validationErrors;
    }
}