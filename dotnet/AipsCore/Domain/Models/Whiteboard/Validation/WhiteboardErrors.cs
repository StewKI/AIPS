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

    public static ValidationError OnlyOwnerCanBanOtherUsers(UserId currentUserId)
    {
        string code = "only_owner_can_ban_other_users";
        string message = $"Only owner of whiteboard can ban other users. Current user id: '{currentUserId}' is not the owner.";
        
        return CreateValidationError(code, message);
    }
     
    public static ValidationError OnlyOwnerCanUnbanOtherUsers(UserId currentUserId)
    {
        string code = "only_owner_can_unban_other_users";
        string message = $"Only owner of whiteboard can unban other users. Current user id: '{currentUserId}' is not the owner.";
        
        return CreateValidationError(code, message);
    }
    
    public static ValidationError OnlyOwnerCanKickOtherUsers(UserId currentUserId)
    {
        string code = "only_owner_can_unban_other_users";
        string message = $"Only owner of whiteboard can kick other users. Current user id: '{currentUserId}' is not the owner.";
        
        return CreateValidationError(code, message);
    }
}