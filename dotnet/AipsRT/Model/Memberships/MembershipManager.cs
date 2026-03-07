using AipsCore.Domain.Models.WhiteboardMembership.Enums;

namespace AipsRT.Model.Memberships;

public class MembershipManager
{
    private readonly IServiceScopeFactory _scopeFactory;

    public MembershipManager(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task<WhiteboardMembershipStatus> GetMembershipStatus(Guid whiteboardId, Guid userId)
    {
        var membershipService = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<MembershipService>();
        return await membershipService.GetMembershipStatus(whiteboardId, userId);
    }
}