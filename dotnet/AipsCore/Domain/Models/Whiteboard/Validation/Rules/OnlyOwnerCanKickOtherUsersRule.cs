using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Domain.Models.Whiteboard.Validation.Rules;

public class OnlyOwnerCanKickOtherUsersRule : AbstractRule
{
    private readonly Whiteboard _whiteboard;
    private readonly UserId _userId;

    public OnlyOwnerCanKickOtherUsersRule(Whiteboard whiteboard, UserId userId)
    {
        _whiteboard = whiteboard;
        _userId = userId;
    }
    
    protected override string ErrorCode => "only_owner_can_kick_other_users";
    protected override string ErrorMessage => $"Only owner of whiteboard can kick other users. Current user id: '{_userId.IdValue}' is not the owner.";
    public override bool Validate()
    {
        return _whiteboard.WhiteboardOwnerId.IdValue == _userId.IdValue;
    }
}