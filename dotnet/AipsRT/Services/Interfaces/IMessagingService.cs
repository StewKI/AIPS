using AipsCore.Application.Models.Shape.Command.CreateTextShape;
using AipsCore.Application.Models.Shape.Command.MoveShape;
using AipsRT.Model.Whiteboard.Shapes;

namespace AipsRT.Services.Interfaces;

public interface IMessagingService
{
    Task CreatedRectangle(Guid whiteboardId, Rectangle rectangle);
    Task CreatedArrow(Guid whiteboardId, Arrow arrow);
    Task CreateLine(Guid whiteboardId, Line line);
    Task CreateTextShape(Guid whiteboardId, TextShape textShape);

    Task MoveShape(MoveShapeCommand moveShape);
}