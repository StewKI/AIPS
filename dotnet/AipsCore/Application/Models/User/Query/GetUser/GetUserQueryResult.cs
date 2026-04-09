using AipsCore.Application.Abstract.Query;

namespace AipsCore.Application.Models.User.Query.GetUser;

public record GetUserQueryResult(string Id, string Email, string UserName) : IQueryResult;