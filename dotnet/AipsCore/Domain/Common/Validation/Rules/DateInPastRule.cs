using AipsCore.Domain.Abstract.Rule;

namespace AipsCore.Domain.Common.Validation.Rules;

public class DateInPastRule : AbstractRule
{
    private readonly DateTime? _date;
    private readonly DateTime _now;

    public DateInPastRule(DateTime? date) 
    {
        _date = date;
        _now = DateTime.Now;
    }
    
    protected override string ErrorCode => "date_in_past";
    protected override string ErrorMessage => "Date must be in the past";
    public override bool Validate()
    {
        if (_date is not null)
        {
            return _date < _now;
        }

        return true;
    }
}