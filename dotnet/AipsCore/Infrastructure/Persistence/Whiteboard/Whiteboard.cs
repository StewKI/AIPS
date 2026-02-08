using System.ComponentModel.DataAnnotations;

namespace AipsCore.Infrastructure.Persistence.Whiteboard;

public class Whiteboard
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid WhiteboardOwnerId { get; set; }
    
    [Required]
    [MaxLength(8)]
    [MinLength(8)]
    public string Code { get; set; } = null!;
    
    [Required]
    [MaxLength(32)]
    [MinLength(3)]
    public string Title { get; set; } = null!;
}