using AipsCore.Domain.Abstract.Rule;

namespace AipsCore.Domain.Common.Validation.Rules;

public class DateInFutureRule : AbstractRule
{
    private readonly DateTime _date;
    private readonly DateTime _now;
    
    public DateInFutureRule(DateTime date) 
    {
        _date = date;
        _now = DateTime.Now;
    }
    
    protected override string ErrorCode => "date_in_future";
    protected override string ErrorMessage => "Date must be in the future";
    public override bool Validate()
    {
        return _date > _now;
    }
}