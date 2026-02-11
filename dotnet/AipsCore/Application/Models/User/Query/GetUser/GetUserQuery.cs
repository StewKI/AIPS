using AipsCore.Application.Abstract.Query;

namespace AipsCore.Application.Models.User.Query.GetUser;

public record GetUserQuery(string UserId) : IQuery<Infrastructure.Persistence.User.User>;