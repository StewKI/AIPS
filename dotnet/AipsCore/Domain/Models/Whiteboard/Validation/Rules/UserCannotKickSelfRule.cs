using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Domain.Models.Whiteboard.Validation.Rules;

public class UserCannotKickSelfRule : AbstractRule
{
    private readonly Whiteboard _whiteboard;
    private readonly UserId _kickedUserId;

    public UserCannotKickSelfRule(Whiteboard whiteboard, UserId kickedUserId)
    {
        _whiteboard = whiteboard;
        _kickedUserId = kickedUserId;
    }

    protected override string ErrorCode => "user_cannot_kick_self";
    protected override string ErrorMessage => "User cannot kick self";
    public override bool Validate()
    {
        return _whiteboard.WhiteboardOwnerId.IdValue != _kickedUserId.IdValue;
    }
}