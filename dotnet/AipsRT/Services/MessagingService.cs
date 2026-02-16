using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Common.Message.AddArrow;
using AipsCore.Application.Common.Message.AddLine;
using AipsCore.Application.Common.Message.AddRectangle;
using AipsCore.Application.Common.Message.AddTextShape;
using AipsCore.Application.Common.Message.MoveShape;
using AipsCore.Application.Models.Shape.Command.CreateArrow;
using AipsCore.Application.Models.Shape.Command.CreateLine;
using AipsCore.Application.Models.Shape.Command.CreateRectangle;
using AipsCore.Application.Models.Shape.Command.CreateTextShape;
using AipsCore.Application.Models.Shape.Command.MoveShape;
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

    public async Task CreatedArrow(Guid whiteboardId, Arrow arrow)
    {
        var command = new CreateArrowCommand(
            arrow.Id.ToString(),
            whiteboardId.ToString(),
            arrow.OwnerId.ToString(),
            arrow.Position.X,
            arrow.Position.Y,
            arrow.Color,
            arrow.EndPosition.X,
            arrow.EndPosition.Y,
            arrow.Thickness);

        var message = new AddArrowMessage(command);
        
        await _messagePublisher.PublishAsync(message);
    }

    public async Task CreateLine(Guid whiteboardId, Line line)
    {
        var command = new CreateLineCommand(
            line.Id.ToString(),
            whiteboardId.ToString(),
            line.OwnerId.ToString(),
            line.Position.X,
            line.Position.Y,
            line.Color,
            line.EndPosition.X,
            line.EndPosition.Y,
            line.Thickness);

        var message = new AddLineMessage(command);
        
        await _messagePublisher.PublishAsync(message);
    }

    public async Task CreateTextShape(Guid whiteboardId, TextShape textShape)
    {
        var command = new CreateTextShapeCommand(
            textShape.Id.ToString(),
            whiteboardId.ToString(),
            textShape.OwnerId.ToString(),
            textShape.Position.X,
            textShape.Position.Y,
            textShape.Color,
            textShape.TextValue,
            textShape.TextSize);

        var message = new AddTextShapeMessage(command);
        
        await _messagePublisher.PublishAsync(message);
    }

    public async Task MoveShape(MoveShapeCommand moveShape)
    {
        var message = new MoveShapeMessage(moveShape);
        await _messagePublisher.PublishAsync(message);
    }
}