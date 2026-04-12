using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Models.User.ValueObjects;

namespace AipsCore.Domain.Models.User.Validation.Rules;

public class UserCanOnlyLogOutHimselfRule : AbstractRule, IRuleMetadata
{
    private readonly UserId _userId;
    private readonly UserId _tokenHolderId;

    public UserCanOnlyLogOutHimselfRule(UserId userId, UserId tokenHolderId)
    {
        _userId = userId;
        _tokenHolderId = tokenHolderId;
    }

    public UserCanOnlyLogOutHimselfRule(UserId userId, string tokenHolderId)
        : this(userId, new UserId(tokenHolderId))
    {
    }

    protected override string ErrorCode => ErrorCodeString;
    protected override string ErrorMessage => "User can only log out himself.";
    public override bool Validate()
    {
        return _userId.IdValue == _tokenHolderId.IdValue;
    }

    public static string ErrorCodeString => "user_can_only_log_out_himself";
}