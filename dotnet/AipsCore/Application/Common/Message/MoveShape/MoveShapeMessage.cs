using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Models.Shape.Command.MoveShape;

namespace AipsCore.Application.Common.Message.MoveShape;

public record MoveShapeMessage(Guid WhiteboardId, MoveShapeCommand Command) : IMessage
{
    public Guid? GetWhiteboardId()
    {
        return WhiteboardId;
    }
}