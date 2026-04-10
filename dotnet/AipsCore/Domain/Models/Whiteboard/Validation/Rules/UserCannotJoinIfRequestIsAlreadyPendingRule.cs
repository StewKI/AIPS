using AipsCore.Domain.Abstract.Rule;

namespace AipsCore.Domain.Models.Whiteboard.Validation.Rules;

public class UserCannotJoinIfRequestIsAlreadyPendingRule : AbstractRule
{
    private readonly WhiteboardMembership.WhiteboardMembership _whiteboardMembership;

    public UserCannotJoinIfRequestIsAlreadyPendingRule(WhiteboardMembership.WhiteboardMembership whiteboardMembership)
    {
        _whiteboardMembership = whiteboardMembership;
    }

    protected override string ErrorCode => "join_request_already_pending";
    protected override string ErrorMessage => "User cannot join if request is already pending";
    public override bool Validate()
    {
        return !_whiteboardMembership.IsPending();
    }
}