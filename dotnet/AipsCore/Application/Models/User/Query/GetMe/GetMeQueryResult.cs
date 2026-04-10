using AipsCore.Application.Abstract.Query;

namespace AipsCore.Application.Models.User.Query.GetMe;

public sealed record GetMeQueryResult(string UserId, string UserName) : IQueryResult;