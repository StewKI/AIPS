using AipsCore.Application.Abstract;
using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Models.Shape.Command.CreateLine;

namespace AipsCore.Application.Common.Message.AddLine;

public record AddLineMessage(CreateLineCommand Command) : IMessage, IWhiteboardAwareContext
{
    public Guid GetWhiteboardId()
    {
        return Guid.Parse(Command.WhiteboardId);
    }
}