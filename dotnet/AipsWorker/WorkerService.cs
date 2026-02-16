using System.Reflection;
using AipsCore.Application.Abstract;
using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Common.Message.TestMessage;
using AipsCore.Domain.Common.Validation;
using AipsWorker.Utilities;
using Microsoft.Extensions.Hosting;

namespace AipsWorker;

public class WorkerService : BackgroundService
{
    private readonly IDispatcher _dispatcher;
    private readonly SubscribeMethodUtility _subscribeMethodUtility;

    public WorkerService(IMessageSubscriber subscriber, IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        _subscribeMethodUtility = new SubscribeMethodUtility(subscriber);
    }

    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        
        var messageTypes = GetAllMessageTypes();

        foreach (var messageType in messageTypes)
        {
            var handleMethod = GetMessageHandleMethod(messageType);
            
            await _subscribeMethodUtility.SubscribeToMessageTypeAsync(messageType, this, handleMethod);
        }
    }

    private IReadOnlyCollection<Type> GetAllMessageTypes()
    {
        var messageInterface = typeof(IMessage);
        var assembly = messageInterface.Assembly;

        return assembly
            .GetTypes()
            .Where(t =>
                !t.IsAbstract &&
                !t.IsInterface &&
                messageInterface.IsAssignableFrom(t))
            .ToList();
    }

    private async Task HandleMessage<T>(T message, CancellationToken ct) where T : IMessage
    {
        try
        {
            await _dispatcher.Execute(message, ct);
        }
        catch (ValidationException validationException)
        {
            Console.WriteLine("===Validation Exception: ");
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