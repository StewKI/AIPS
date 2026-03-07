using AipsCore.Application.Abstract;
using AipsCore.Application.Models.Whiteboard.Query.GetMembershipStatus;
using AipsCore.Domain.Models.WhiteboardMembership.Enums;

namespace AipsRT.Model.Memberships;

public class MembershipService
{
    private readonly IDispatcher _dispatcher;

    public MembershipService(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public async Task<WhiteboardMembershipStatus> GetMembershipStatus(Guid whiteboardId, Guid userId)
    {
        var query = new GetMembershipStatusQuery(whiteboardId.ToString(), userId.ToString());
        return await _dispatcher.Execute(query);
    }
}