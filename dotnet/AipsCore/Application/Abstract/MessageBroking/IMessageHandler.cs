namespace AipsCore.Application.Abstract.MessageBroking;

public interface IMessageHandler<TMessage> where TMessage : IMessage
{
    Task Handle(TMessage message, CancellationToken cancellationToken);
}