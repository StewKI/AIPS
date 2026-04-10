using AipsCore.Application.Abstract.Query;
using AipsCore.Domain.Models.WhiteboardMembership.Enums;

namespace AipsCore.Application.Models.Whiteboard.Query.GetMembershipStatus;

public sealed record GetMembershipStatusQuery(string WhiteboardId, string UserId)
    : IQuery<GetMembershipStatusQueryResult>;