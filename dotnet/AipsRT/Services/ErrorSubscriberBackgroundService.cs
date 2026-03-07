using AipsCore.Application.Abstract;
using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Common.Message.ErrorMessage;

namespace AipsRT.Services;

public class ErrorSubscriberBackgroundService : BackgroundService
{
    private readonly IMessageSubscriber _subscriber;
    private readonly IDispatcher _dispatcher;

    public ErrorSubscriberBackgroundService(IMessageSubscriber subscriber, IDispatcher dispatcher)
    {
        _subscriber = subscriber;
        _dispatcher = dispatcher;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _subscriber.SubscribeAsync<ErrorMessage>(async (errorMessage, ct) =>
        {
            await _dispatcher.Execute(errorMessage, ct);
        });
    }
}