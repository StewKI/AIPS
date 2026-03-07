using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Models.Shape.Command.CreateRectangle;

namespace AipsCore.Application.Common.Message.AddRectangle;

public record AddRectangleMessage(CreateRectangleCommand Command) : IMessage
{
    public Guid? GetWhiteboardId()
    {
        return Guid.Parse(Command.WhiteboardId);
    }
}