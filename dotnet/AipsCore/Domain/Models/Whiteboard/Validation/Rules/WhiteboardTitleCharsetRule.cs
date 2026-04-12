using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Utility.Text;

namespace AipsCore.Domain.Models.Whiteboard.Validation.Rules;

public class WhiteboardTitleCharsetRule : CharsetRule, IRuleMetadata
{
    public WhiteboardTitleCharsetRule(string stringValue) 
        : base(stringValue, GetWhiteboardTitleCharset())
    {
        
    }
    
    private static char[] GetWhiteboardTitleCharset()
    {
        var alphanumericCharset = Charset.GetAlphanumericCharset();
        return [..alphanumericCharset, '_', ' ', '/'];
    }

    protected override string ErrorCode => ErrorCodeString;

    protected override string ErrorMessage =>
        "Whiteboard title contains invalid characters, only alphanumeric characters, '_', ' ', '/' are allowed";

    public static string ErrorCodeString => "whiteboard_title_charset";
}