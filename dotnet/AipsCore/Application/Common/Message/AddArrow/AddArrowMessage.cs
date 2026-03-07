using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Models.Shape.Command.CreateArrow;

namespace AipsCore.Application.Common.Message.AddArrow;

public record AddArrowMessage(CreateArrowCommand Command) : IMessage
{
    public Guid? GetWhiteboardId()
    {
        return Guid.Parse(Command.WhiteboardId);
    }
}