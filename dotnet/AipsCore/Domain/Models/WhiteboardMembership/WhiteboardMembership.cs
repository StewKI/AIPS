using AipsCore.Domain.Abstract;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsCore.Domain.Models.WhiteboardMembership.ValueObjects;

namespace AipsCore.Domain.Models.WhiteboardMembership;

public partial class WhiteboardMembership : DomainModel<WhiteboardMembershipId>
{
    public WhiteboardId WhiteboardId { get; private set; }
    public UserId UserId { get; private set; }
    public WhiteboardMembershipIsBanned IsBanned { get; private set; }
    public WhiteboardMembershipEditingEnabled EditingEnabled { get; private set; }
    public WhiteboardMembershipCanJoin CanJoin { get; private set; }
    public WhiteboardMembershipLastInteractedAt LastInteractedAt { get; private set; }

    public WhiteboardMembership(
        WhiteboardMembershipId id,
        Whiteboard.Whiteboard owner, 
        User.User user,
        WhiteboardMembershipIsBanned isBanned,
        WhiteboardMembershipEditingEnabled editingEnabled,
        WhiteboardMembershipCanJoin canJoin,
        WhiteboardMembershipLastInteractedAt lastInteractedAt)
        : base(id)
    {
        WhiteboardId = owner.Id;
        UserId = user.Id;
        IsBanned = isBanned;
        EditingEnabled = editingEnabled;
        CanJoin = canJoin;
        LastInteractedAt = lastInteractedAt;
    }
    
    public WhiteboardMembership(
        WhiteboardMembershipId id,
        WhiteboardId ownerId, 
        UserId userId,
        WhiteboardMembershipIsBanned isBanned,
        WhiteboardMembershipEditingEnabled editingEnabled,
        WhiteboardMembershipCanJoin canJoin,
        WhiteboardMembershipLastInteractedAt lastInteractedAt)
        : base(id)
    {
        WhiteboardId = ownerId;
        UserId = userId;
        IsBanned = isBanned;
        EditingEnabled = editingEnabled;
        CanJoin = canJoin;
        LastInteractedAt = lastInteractedAt;
    }
    
    public static WhiteboardMembership Create(
        string id, 
        string ownerId,
        string userId,
        bool isBanned,
        bool editingEnabled,
        bool canJoin,
        DateTime lastInteractedAt)
    {
        var whiteboardMembershipId = new WhiteboardMembershipId(id);
        var whiteboardId = new WhiteboardId(ownerId);
        var userIdVo = new UserId(userId);
        var isBannedVo = new WhiteboardMembershipIsBanned(isBanned);
        var editingEnabledVo = new WhiteboardMembershipEditingEnabled(editingEnabled);
        var canJoinVo = new WhiteboardMembershipCanJoin(canJoin);
        var lastInteractedAtVo = new WhiteboardMembershipLastInteractedAt(lastInteractedAt);
        
        return new WhiteboardMembership(
            whiteboardMembershipId, 
            whiteboardId, 
            userIdVo, 
            isBannedVo, 
            editingEnabledVo, 
            canJoinVo, 
            lastInteractedAtVo);
    }
    
    public static WhiteboardMembership Create(
        string ownerId,
        string userId,
        bool isBanned,
        bool editingEnabled,
        bool canJoin,
        DateTime lastInteractedAt)
    {
        var whiteboardMembershipId = WhiteboardMembershipId.Any();
        var whiteboardId = new WhiteboardId(ownerId);
        var userIdVo = new UserId(userId);
        var isBannedVo = new WhiteboardMembershipIsBanned(isBanned);
        var editingEnabledVo = new WhiteboardMembershipEditingEnabled(editingEnabled);
        var canJoinVo = new WhiteboardMembershipCanJoin(canJoin);
        var lastInteractedAtVo = new WhiteboardMembershipLastInteractedAt(lastInteractedAt);
        
        return new WhiteboardMembership(
            whiteboardMembershipId, 
            whiteboardId, 
            userIdVo, 
            isBannedVo, 
            editingEnabledVo, 
            canJoinVo, 
            lastInteractedAtVo);
    }
}