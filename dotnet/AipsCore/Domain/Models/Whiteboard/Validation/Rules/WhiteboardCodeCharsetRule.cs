using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Utility.Text;

namespace AipsCore.Domain.Models.Whiteboard.Validation.Rules;

public class WhiteboardCodeCharsetRule : CharsetRule, IRuleMetadata
{
    public WhiteboardCodeCharsetRule(string stringValue)
        : base(stringValue, GetWhiteboardCodeCharset())
    {
    }

    private static char[] GetWhiteboardCodeCharset()
    {
        return Charset.GetNumericCharset();
    }

    protected override string ErrorCode => ErrorCodeString;
    protected override string ErrorMessage => "Whiteboard code must contain only numbers";
    public static string ErrorCodeString => "whiteboard_code_charset";
}