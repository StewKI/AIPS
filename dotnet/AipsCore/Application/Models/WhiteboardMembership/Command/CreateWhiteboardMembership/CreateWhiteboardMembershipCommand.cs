using AipsCore.Application.Abstract.Command;
using AipsCore.Domain.Models.WhiteboardMembership.ValueObjects;

namespace AipsCore.Application.Models.WhiteboardMembership.Command.CreateWhiteboardMembership;

public record CreateWhiteboardMembershipCommand(
    string WhiteboardId, 
    bool IsBanned,
    bool EditingEnabled,
    bool CanJoin)
    : ICommand<WhiteboardMembershipId>;