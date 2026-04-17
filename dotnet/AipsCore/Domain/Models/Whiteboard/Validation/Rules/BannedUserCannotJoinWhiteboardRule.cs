using AipsCore.Domain.Abstract.Rule;

namespace AipsCore.Domain.Models.Whiteboard.Validation.Rules;

public class BannedUserCannotJoinWhiteboardRule : AbstractRule
{
    private readonly WhiteboardMembership.WhiteboardMembership? _whiteboardMembership;

    public BannedUserCannotJoinWhiteboardRule(WhiteboardMembership.WhiteboardMembership? whiteboardMembership)
    {
        _whiteboardMembership = whiteboardMembership;
    }
    
    protected override string ErrorCode => "banned_user_cannot_join_whiteboard";
    protected override string ErrorMessage => $"User with id: '{_whiteboardMembership?.UserId.IdValue}' is banned from joining whiteboard with id: '{_whiteboardMembership?.WhiteboardId.IdValue}'.";
    public override bool Validate()
    {
        if (_whiteboardMembership is null)
        {
            return true;
        }
        
        return !_whiteboardMembership.IsBanned();
    }
}