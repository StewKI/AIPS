using System.ComponentModel.DataAnnotations;

namespace AipsCore.Infrastructure.Persistence.WhiteboardMembership;

public class WhiteboardMembership 
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid WhiteboardId { get; set; }

    public Whiteboard.Whiteboard? Whiteboard { get; set; } = null!;
    
    public Guid UserId { get; set; }

    public User.User? User { get; set; } = null!;
    
    public bool IsBanned { get; set; }
    
    public bool EditingEnabled { get; set; }
    
    public bool CanJoin { get; set; }
    
    public DateTime LastInteractedAt { get; set; }
}