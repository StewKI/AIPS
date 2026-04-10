using AipsCore.Application.Abstract.Query;

namespace AipsCore.Application.Models.User.Query.GetUser;

public sealed record GetUserQueryResult(string Id, string Email, string UserName) : IQueryResult;