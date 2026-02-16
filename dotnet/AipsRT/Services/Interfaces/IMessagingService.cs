using AipsRT.Model.Whiteboard.Shapes;

namespace AipsRT.Services.Interfaces;

public interface IMessagingService
{
    Task CreatedRectangle(Guid whiteboardId, Rectangle rectangle);
}