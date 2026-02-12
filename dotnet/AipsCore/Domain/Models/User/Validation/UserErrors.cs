using AipsCore.Domain.Abstract.Validation;
using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.User.External;
using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Domain.Models.User.Validation;

public class UserErrors : AbstractErrors<User, UserId>
{
    public static ValidationError InvalidCredentials()
    {
        string code = "invalid_credentials";
        string message = "Invalid credentials";

        return CreateValidationError(code, message);
    }
    
    public static ValidationError RoleDoesNotExist(string name)
        {
            string code = "user_role_does_not_exist";
            string message = $"Role '{name}' does not exist";
    
            return CreateValidationError(code, message);
        }
}