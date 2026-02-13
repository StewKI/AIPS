using System.Runtime.Serialization;

namespace AipsCore.Application.Abstract;

public interface IMessagePublisher
{
    Task PublishAsync<T>(string exchange, string routeKey, T message, CancellationToken cancellationToken = default);
}