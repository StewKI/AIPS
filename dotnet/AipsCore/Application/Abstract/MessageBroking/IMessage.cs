namespace AipsCore.Application.Abstract.MessageBroking;

public interface IMessage
{
    Guid? GetWhiteboardId();
};