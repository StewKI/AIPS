using AipsCore.Domain.Abstract.Rule;

namespace AipsCore.Domain.Models.Whiteboard.Validation.Rules;

public class UserCannotJoinPrivateWhiteboardRule : AbstractRule
{
    private readonly Whiteboard _whiteboard;

    public UserCannotJoinPrivateWhiteboardRule(Whiteboard whiteboard)
    {
        _whiteboard = whiteboard;
    }

    protected override string ErrorCode => "user_cannot_join_private_whiteboard";
    protected override string ErrorMessage => $"Whiteboard is private.";
    public override bool Validate()
    {
        return !_whiteboard.IsPrivate();
    }
}