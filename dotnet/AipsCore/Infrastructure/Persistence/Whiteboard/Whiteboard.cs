using System.ComponentModel.DataAnnotations;
using AipsCore.Domain.Models.Whiteboard.Enums;

namespace AipsCore.Infrastructure.Persistence.Whiteboard;

public class Whiteboard
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid OwnerId { get; set; }

    public User.User Owner { get; set; } = null!;
    
    [Required]
    [MaxLength(8)]
    [MinLength(8)]
    public string Code { get; set; } = null!;
    
    [Required]
    [MaxLength(32)]
    [MinLength(3)]
    public string Title { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? DeletedAt { get; set; }
    
    public int MaxParticipants { get; set; }
    
    public WhiteboardJoinPolicy JoinPolicy { get; set; }
    
    public WhiteboardState State { get; set; }
    
    public ICollection<Shape.Shape> Shapes { get; set; } = new List<Shape.Shape>();
    
    public ICollection<WhiteboardMembership.WhiteboardMembership> Memberships { get; set; } = new List<WhiteboardMembership.WhiteboardMembership>();
}