using AipsCore.Domain.Abstract.Validation;
using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Domain.Models.Whiteboard.Validation;

public class WhiteboardErrors : AbstractErrors<Whiteboard, WhiteboardId>
{
    public static ValidationError NotFound(WhiteboardCode whiteboardCode)
    {
        const string code = "not_found";
        string message = $"Whiteboard with code '{whiteboardCode.CodeValue}' was not found!";
        
        return CreateValidationError(code,message);
    }
}