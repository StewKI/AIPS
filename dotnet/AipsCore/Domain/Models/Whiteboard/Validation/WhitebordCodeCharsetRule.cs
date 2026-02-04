using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Utility.Text;

namespace AipsCore.Domain.Models.Whiteboard.Validation;

public class WhitebordCodeCharsetRule : CharsetRule
{
    public WhitebordCodeCharsetRule(string stringValue)
        : base(stringValue, GetWhiteboardCodeCharset())
    {
    }

    private static char[] GetWhiteboardCodeCharset()
    {
        return Charset.GetNumericCharset();
    }

    protected override string ErrorCode => "whiteboard_code_charset";
    protected override string ErrorMessage => "Whiteboard code must contain only numbers";
}