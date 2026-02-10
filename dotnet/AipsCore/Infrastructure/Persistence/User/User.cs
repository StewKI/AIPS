using System.ComponentModel.DataAnnotations;

namespace AipsCore.Infrastructure.Persistence.User;


public class User
{
    [Key]
    public Guid Id { get; set; }
    
    [Required] 
    [MaxLength(255)] 
    public string Username { get; set; } = null!;
    
    [Required] 
    [MaxLength(255)] 
    public string Email { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? DeletedAt { get; set; }
    
    public ICollection<Shape.Shape> Shapes { get; set; } = new List<Shape.Shape>();
    
    public ICollection<Whiteboard.Whiteboard> Whiteboards { get; set; } = new List<Whiteboard.Whiteboard>();
    
    public ICollection<WhiteboardMembership.WhiteboardMembership> Memberships { get; set; } = new List<WhiteboardMembership.WhiteboardMembership>();
}