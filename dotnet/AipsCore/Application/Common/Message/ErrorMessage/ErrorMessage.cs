using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Domain.Common.Validation;

namespace AipsCore.Application.Common.Message.ErrorMessage;

public record ErrorMessage(Guid WhiteboardId, ICollection<ValidationError> Errors) : IMessage
{
    public Guid? GetWhiteboardId()
    {
        return WhiteboardId;
    }
}