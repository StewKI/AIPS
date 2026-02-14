namespace AipsCore.Application.Abstract.MessageBroking;

public interface IMessageSubscriber
{
    Task SubscribeAsync<T>(
        Func<T, CancellationToken, Task> handler
    );
}