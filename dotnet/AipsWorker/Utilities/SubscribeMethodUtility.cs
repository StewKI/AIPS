using System.Reflection;
using AipsCore.Application.Abstract.MessageBroking;

namespace AipsWorker.Utilities;

public class SubscribeMethodUtility
{
    private readonly IMessageSubscriber _subscriber;

    public SubscribeMethodUtility(IMessageSubscriber subscriber)
    {
        _subscriber = subscriber;
    }
    
    public async Task SubscribeToMessageTypeAsync(
        Type messageType,
        object targetInstance,
        MethodInfo handlerMethod)
    {
        var subscribeMethod = GetGenericSubscribeMethod(messageType);
        var handlerDelegate = CreateHandlerDelegate(messageType, targetInstance, handlerMethod);

        var task = (Task)subscribeMethod.Invoke(
            _subscriber,
            new object[] { handlerDelegate })!;

        await task;
    }
    
    private MethodInfo GetGenericSubscribeMethod(Type messageType)
    {
        var method = typeof(IMessageSubscriber)
            .GetMethod(nameof(IMessageSubscriber.SubscribeAsync))!;

        return method.MakeGenericMethod(messageType);
    }
    
    private Delegate CreateHandlerDelegate(
        Type messageType,
        object targetInstance,
        MethodInfo handlerMethod)
    {
        var delegateType = typeof(Func<,,>)
            .MakeGenericType(messageType, typeof(CancellationToken), typeof(Task));

        return Delegate.CreateDelegate(delegateType, targetInstance, handlerMethod);
    }
}