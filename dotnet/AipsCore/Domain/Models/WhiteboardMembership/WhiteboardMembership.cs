using AipsCore.Domain.Abstract;
using AipsCore.Domain.Models.User.ValueObjects;
using AipsCore.Domain.Models.Whiteboard.ValueObjects;
using AipsCore.Domain.Models.WhiteboardMembership.Enums;
using AipsCore.Domain.Models.WhiteboardMembership.ValueObjects;

namespace AipsCore.Domain.Models.WhiteboardMembership;

public partial class WhiteboardMembership : DomainModel<WhiteboardMembershipId>
{
    public WhiteboardId WhiteboardId { get; private set; }
    public UserId UserId { get; private set; }
    public WhiteboardMembershipEditingEnabled EditingEnabled { get; private set; }
    public WhiteboardMembershipStatus Status { get; private set; }
    public WhiteboardMembershipLastInteractedAt LastInteractedAt { get; private set; }

    public WhiteboardMembership(
        WhiteboardMembershipId id,
        Whiteboard.Whiteboard whiteboard, 
        User.User user,
        WhiteboardMembershipEditingEnabled editingEnabled,
        WhiteboardMembershipStatus status,
        WhiteboardMembershipLastInteractedAt lastInteractedAt)
        : base(id)
    {
        WhiteboardId = whiteboard.Id;
        UserId = user.Id;
        EditingEnabled = editingEnabled;
        Status = status;
        LastInteractedAt = lastInteractedAt;
    }
    
    public WhiteboardMembership(
        WhiteboardMembershipId id,
        WhiteboardId whiteboardId, 
        UserId userId,
        WhiteboardMembershipEditingEnabled editingEnabled,
        WhiteboardMembershipStatus status,
        WhiteboardMembershipLastInteractedAt lastInteractedAt)
        : base(id)
    {
        WhiteboardId = whiteboardId;
        UserId = userId;
        EditingEnabled = editingEnabled;
        Status = status;
        LastInteractedAt = lastInteractedAt;
    }
    
    public static WhiteboardMembership Create(
        string id, 
        string whiteboardId,
        string userId,
        bool editingEnabled,
        WhiteboardMembershipStatus status,
        DateTime lastInteractedAt)
    {
        var whiteboardMembershipId = new WhiteboardMembershipId(id);
        var whiteboardIdVo = new WhiteboardId(whiteboardId);
        var userIdVo = new UserId(userId);
        var editingEnabledVo = new WhiteboardMembershipEditingEnabled(editingEnabled);
        var lastInteractedAtVo = new WhiteboardMembershipLastInteractedAt(lastInteractedAt);
        
        return new WhiteboardMembership(
            whiteboardMembershipId, 
            whiteboardIdVo, 
            userIdVo,
            editingEnabledVo, 
            status,
            lastInteractedAtVo);
    }
    
    public static WhiteboardMembership Create(
        string whiteboardId,
        string userId,
        bool editingEnabled,
        WhiteboardMembershipStatus status,
        DateTime lastInteractedAt)
    {
        var whiteboardMembershipId = WhiteboardMembershipId.Any();
        var whiteboardIdVo = new WhiteboardId(whiteboardId);
        var userIdVo = new UserId(userId);
        var editingEnabledVo = new WhiteboardMembershipEditingEnabled(editingEnabled);
        var lastInteractedAtVo = new WhiteboardMembershipLastInteractedAt(lastInteractedAt);
        
        return new WhiteboardMembership(
            whiteboardMembershipId, 
            whiteboardIdVo, 
            userIdVo, 
            editingEnabledVo, 
            status,
            lastInteractedAtVo);
    }
}