using AipsCore.Application.Abstract.Query;
using AipsCore.Domain.Models.WhiteboardMembership.Enums;

namespace AipsCore.Application.Models.Whiteboard.Query.GetMembershipStatus;

public record GetMembershipStatusQueryResult(WhiteboardMembershipStatus Status) : IQueryResult;