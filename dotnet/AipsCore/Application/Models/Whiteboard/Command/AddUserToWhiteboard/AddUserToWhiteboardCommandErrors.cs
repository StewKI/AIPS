using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Application.Models.Whiteboard.Command.AddUserToWhiteboard;

public static class AddUserToWhiteboardCommandErrors
{
    public static ValidationError WhiteboardDoesNotExist(WhiteboardId whiteboardId)
        => new ValidationError(
            Code: "whiteboard_not_exists", 
            Message: $"Whiteboard with id '{whiteboardId}' does not exist.");
}