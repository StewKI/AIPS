namespace AipsCore.Application.Abstract.MessageBroking;

public interface IMessagePublisher
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default);
}