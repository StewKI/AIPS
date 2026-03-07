using AipsCore.Application.Abstract;
using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Domain.Common.Validation;

namespace AipsCore.Application.Common.Message.ErrorMessage;

public record ErrorMessage(Guid WhiteboardId, ICollection<ValidationError> Errors) : IMessage, IWhiteboardAwareContext
{
    public Guid GetWhiteboardId()
    {
        return WhiteboardId;
    }
}