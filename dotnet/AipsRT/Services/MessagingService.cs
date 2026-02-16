using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Common.Message.AddRectangle;
using AipsCore.Application.Models.Shape.Command.CreateRectangle;
using AipsRT.Model.Whiteboard.Shapes;
using AipsRT.Services.Interfaces;

namespace AipsRT.Services;

public class MessagingService : IMessagingService
{
    private readonly IMessagePublisher _messagePublisher;

    public MessagingService(IMessagePublisher messagePublisher)
    {
        _messagePublisher = messagePublisher;
    }
    
    public async Task CreatedRectangle(Guid whiteboardId, Rectangle rectangle)
    {
        var command = new CreateRectangleCommand(
            rectangle.Id.ToString(),
            whiteboardId.ToString(),
            rectangle.OwnerId.ToString(),
            rectangle.Position.X,
            rectangle.Position.Y,
            rectangle.Color,
            rectangle.EndPosition.X,
            rectangle.EndPosition.Y,
            rectangle.BorderThickness
        );

        var message = new AddRectangleMessage(command);
        
        await _messagePublisher.PublishAsync(message);
    }
}