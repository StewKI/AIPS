using AipsCore.Application.Abstract.Query;

namespace AipsCore.Application.Models.User.Query.GetUser;

public sealed record GetUserQuery(string UserId) : IQuery<GetUserQueryResult>;