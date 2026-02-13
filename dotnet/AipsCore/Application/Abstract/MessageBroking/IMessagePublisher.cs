namespace AipsCore.Application.Abstract.MessageBroking;

public interface IMessagePublisher
{
    Task PublishAsync<T>(string exchange, string routeKey, T message, CancellationToken cancellationToken = default);
}