using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Utility.Text;

namespace AipsCore.Domain.Models.User.Validation;

public class UsernameCharsetRule : CharsetRule
{
    public UsernameCharsetRule(string stringValue) 
        : base(stringValue, GetUsernameCharset())
    {
    }

    private static char[] GetUsernameCharset()
    {
        var alphanumericCharset = Charset.GetAlphanumericCharset();

        char[] usernameCharset = [..alphanumericCharset, '_'];

        return usernameCharset;
    }

    protected override string ErrorCode => "username_charset";

    protected override string ErrorMessage =>
        "Username contains invalid characters, only alphanumeric characters and '_' are allowed";
}