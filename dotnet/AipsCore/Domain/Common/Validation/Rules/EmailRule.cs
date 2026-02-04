using System.Net.Mail;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Abstract.Rule;

namespace AipsCore.Domain.Common.Validation.Rules;

public class EmailRule : AbstractRule
{
    protected override string ErrorCode => "email_invalid";
    protected override string ErrorMessage => "Email is not in the valid format";

    private readonly string _emailValue;
    
    public EmailRule(string emailValue)
    {
        _emailValue = emailValue;
    }
    
    public override bool Validate()
    {
        try
        {
            _ = new MailAddress(_emailValue);
        }
        catch
        {
            return false;
        }

        return true;
    }
}