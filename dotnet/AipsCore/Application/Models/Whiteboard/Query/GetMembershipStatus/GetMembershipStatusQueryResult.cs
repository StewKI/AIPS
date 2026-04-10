using AipsCore.Application.Abstract.Query;
using AipsCore.Domain.Models.WhiteboardMembership.Enums;

namespace AipsCore.Application.Models.Whiteboard.Query.GetMembershipStatus;

public sealed record GetMembershipStatusQueryResult(WhiteboardMembershipStatus Status) : IQueryResult;