using AipsCore.Domain.Abstract.Rule;

namespace AipsCore.Domain.Common.Validation.Rules;

public class ColorFormatRule : AbstractRule
{
    private readonly string _colorValue;
    protected override string ErrorCode => "color_format";
    protected override string ErrorMessage => "Color should be in format '#000000'";

    public ColorFormatRule(string colorValue)
    {
        _colorValue = colorValue;
    }
    
    public override bool Validate()
    {
        if (_colorValue.Length != 7) return false;

        if (_colorValue[0] != '#') return false;

        for (int i = 1; i < _colorValue.Length; i++)
        {
            if (!char.IsAsciiHexDigit(_colorValue[i])) return false;    
        }
        
        return true;
    }
}