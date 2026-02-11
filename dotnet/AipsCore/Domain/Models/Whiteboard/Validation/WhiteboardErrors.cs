using AipsCore.Domain.Abstract.Validation;
using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Domain.Models.Whiteboard.Validation;

public class WhiteboardErrors : AbstractErrors<Whiteboard, WhiteboardId>
{
    public static ValidationError UserAlreadyAdded(UserId userId)
    {
        string code = "user_already_added";
        string message = $"User with id '{userId}' already added to this whiteboard";

        return CreateValidationError(code, message);
    }
        
}