using System.Reflection;
using AipsCore.Application.Abstract;
using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Common.Message.ErrorMessage;
using AipsCore.Application.Common.Message.TestMessage;
using AipsCore.Domain.Common.Validation;
using AipsWorker.Utilities;
using Microsoft.Extensions.Hosting;

namespace AipsWorker;

public class WorkerService : BackgroundService
{
    private readonly IDispatcher _dispatcher;
    private readonly IMessageTypesProvider _messageTypesProvider;
    private readonly IMessagePublisher _publisher;
    private readonly SubscribeMethodUtility _subscribeMethodUtility;

    public WorkerService(IMessageSubscriber subscriber, IDispatcher dispatcher, IMessageTypesProvider messageTypesProvider, IMessagePublisher publisher)
    {
        _dispatcher = dispatcher;
        _messageTypesProvider = messageTypesProvider;
        _publisher = publisher;
        _subscribeMethodUtility = new SubscribeMethodUtility(subscriber);
    }

    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        
        var messageTypes = _messageTypesProvider.GetAllMessageTypes();

        foreach (var messageType in messageTypes)
        {
            var handleMethod = GetMessageHandleMethod(messageType);
            
            await _subscribeMethodUtility.SubscribeToMessageTypeAsync(messageType, this, handleMethod);
        }
    }

    private async Task HandleMessage<T>(T message, CancellationToken ct) where T : IMessage
    {
        Console.WriteLine($"*--------{message.GetType().Name}--------*");
        
        try
        {
            await _dispatcher.Execute(message, ct);

            Console.WriteLine("OK!");
        }
        catch (ValidationException validationException)
        {
            if (message is IWhiteboardAwareContext)
            {
                var whiteboardId = ((IWhiteboardAwareContext)message).GetWhiteboardId();
                
                var errorMessage = new ErrorMessage(whiteboardId, validationException.ValidationErrors);

                await _publisher.PublishAsync(errorMessage, ct);
            }
            
            Console.WriteLine("Validation Exception: ");
            foreach (var error in validationException.ValidationErrors)
            {
                Console.WriteLine(" * Code: " + error.Code);
                Console.WriteLine(" * Message: " + error.Message);
                Console.WriteLine("===================");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unhandled Exception: " + ex.Message);
        }
    }
    
    private MethodInfo GetMessageHandleMethod(Type messageType)
    {
        return GetType()
            .GetMethod(nameof(HandleMessage),
                BindingFlags.Instance | BindingFlags.NonPublic)!
            .MakeGenericMethod(messageType);
    }
}