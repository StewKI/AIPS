using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Domain.Models.WhiteboardMembership.Validation.Rules;

public class WhiteboardMembershipExistsRule : AbstractRule
{
    private readonly WhiteboardMembership? _whiteboardMembership;
    private readonly WhiteboardId _whiteboardId;
    private readonly UserId _userId;

    public WhiteboardMembershipExistsRule(WhiteboardMembership? whiteboardMembership, WhiteboardId whiteboardId, UserId userId)
    {
        _whiteboardMembership = whiteboardMembership;
        _whiteboardId = whiteboardId;
        _userId = userId;
    }
    
    protected override string ErrorCode => "whiteboard_membership_does_not_exist";
    protected override string ErrorMessage => $"User with id: '{_userId.IdValue}' is not a member of whiteboard with id: '{_whiteboardId.IdValue}'.";
    public override bool Validate()
    {
        return _whiteboardMembership is not null;
    }
}