using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AipsCore.Infrastructure.Persistence.User;


public class User : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; }
    
    public DateTime? DeletedAt { get; set; }
    
    public ICollection<Shape.Shape> Shapes { get; set; } = new List<Shape.Shape>();
    
    public ICollection<Whiteboard.Whiteboard> Whiteboards { get; set; } = new List<Whiteboard.Whiteboard>();
    
    public ICollection<WhiteboardMembership.WhiteboardMembership> Memberships { get; set; } = new List<WhiteboardMembership.WhiteboardMembership>();
}