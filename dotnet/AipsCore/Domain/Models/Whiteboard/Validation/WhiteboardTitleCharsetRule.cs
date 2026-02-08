using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Utility.Text;

namespace AipsCore.Domain.Models.Whiteboard.Validation;

public class WhiteboardTitleCharsetRule : CharsetRule
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
    
    protected override string ErrorCode => "whiteboard_title_charset";

    protected override string ErrorMessage =>
        "Whiteboard title contains invalid characters, only alphanumeric characters, '_', ' ', '/' are allowed";
}