using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Domain.Models.Whiteboard.Validation.Rules;

public class UserCannotBanSelfRule : AbstractRule
{
    private readonly Whiteboard _whiteboard;
    private readonly UserId _bannedUserId;
    
    public UserCannotBanSelfRule(Whiteboard whiteboard, UserId bannedUserId)
    {
        _whiteboard = whiteboard;
        _bannedUserId = bannedUserId;
    }
    
    protected override string ErrorCode => "user_cannot_ban_self";
    protected override string ErrorMessage => "User cannot ban self";
    public override bool Validate()
    {
        return _whiteboard.WhiteboardOwnerId.IdValue != _bannedUserId.IdValue;
    }
}