using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Common.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Domain.Abstract.Validation;

public abstract class AbstractErrors<TModel, TId>
where TModel : DomainModel<TId> 
where TId : DomainId
{
    public static string GetModelName()
    {
        return typeof(TModel).Name;
    }
    
    public static string Prefix()
    {
        return GetModelName().ToLower();
    }

    protected static string GetFullCode(string code)
    {
        return $"{Prefix()}_{code}";
    }

    protected static ValidationError CreateValidationError(string code, string errorMessage)
    {
        return new ValidationError(GetFullCode(code), errorMessage);
    }

    public static ValidationError NotFound(TId id)
    {
        const string code = "not_found";
        string message = $"{GetModelName()} with id '{id}' was not found!";
        
        return CreateValidationError(code,message);
    }
}