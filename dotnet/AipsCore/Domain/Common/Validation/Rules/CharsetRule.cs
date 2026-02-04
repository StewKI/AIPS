using AipsCore.Domain.Abstract;
using AipsCore.Domain.Abstract.Rule;

namespace AipsCore.Domain.Common.Validation.Rules;

public class CharsetRule : AbstractRule
{
    private readonly string _stringValue;
    private readonly char[] _charset;

    protected CharsetRule(string stringValue, char[] charset)
    {
        _stringValue = stringValue;
        _charset = charset;
    }

    protected override string ErrorCode => "charset";
    protected override string ErrorMessage => $"Forbidden characters in '{ValueObjectName}'";

    public override bool Validate()
    {
        foreach (char character in _stringValue)
        {
            if (!_charset.Contains(character))
            {
                return false;
            }
        }
        return true;
    }
}