using AipsCore.Domain.Abstract.Validation;
using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Domain.Models.User.Validation;

public class UserErrors : AbstractErrors<User, UserId>
{
    public static ValidationError LoginErrorUserNotFoundByEmail(string email)
    {
        string code = "login_error_user_not_found_by_email";
        string message = $"User with email '{email}' not found";

        return CreateValidationError(code, message);
    }
    
    public static ValidationError LoginErrorIncorrectPassword()
    {
        string code = "login_error_incorrect_password";
        string message = $"Incorrect password provided";

        return CreateValidationError(code, message);
    }
}