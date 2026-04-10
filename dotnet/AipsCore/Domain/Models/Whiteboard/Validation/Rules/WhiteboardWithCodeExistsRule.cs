using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;

namespace AipsCore.Domain.Models.Whiteboard.Validation.Rules;

public class WhiteboardWithCodeExistsRule : AbstractRule
{
    private readonly Whiteboard? _whiteboard;
    private readonly WhiteboardCode _whiteboardCode;

    public WhiteboardWithCodeExistsRule(Whiteboard? whiteboard, WhiteboardCode whiteboardCode)
    {
        _whiteboard = whiteboard;
        _whiteboardCode = whiteboardCode;
    }
    
    protected override string ErrorCode => "whiteboard_with_code_does_not_exist";
    protected override string ErrorMessage => $"Whiteboard with code: '{_whiteboardCode.CodeValue}' does not exist.";
    public override bool Validate()
    {
        return _whiteboard is not null;
    }
}