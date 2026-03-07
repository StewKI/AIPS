using AipsCore.Application.Abstract;
using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Models.Shape.Command.CreateArrow;

namespace AipsCore.Application.Common.Message.AddArrow;

public record AddArrowMessage(CreateArrowCommand Command) : IMessage, IWhiteboardAwareContext
{
    public Guid GetWhiteboardId()
    {
        return Guid.Parse(Command.WhiteboardId);
    }
}