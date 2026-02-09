using System.ComponentModel.DataAnnotations;
using AipsCore.Domain.Models.Shape.Enums;

namespace AipsCore.Infrastructure.Persistence.Shape;


public class Shape
{
    [Key]
    public Guid Id { get; set; }
    
    // NAV TO WHITEBOARD
    public Guid WhiteboardId { get; set; }
    public Whiteboard.Whiteboard Whiteboard { get; set; } = null!;
    
    public ShapeType Type { get; set; }
    
    public int PositionX { get; set; }
    public int PositionY { get; set; }
    
    [MaxLength(10)] public string Color { get; set; } = null!;
    
    // END POSITION (Rectangle, Line, Arrow)
    public int? EndPositionX { get; set; }
    public int? EndPositionY { get; set; }
    
    // THICKNESS (Rectangle, Line, Arrow)
    public int? Thickness { get; set; }
    
    // TEXT (Text)
    public string? TextValue { get; set; }
    public int? TextSize { get; set; }
}