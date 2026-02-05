using AipsCore.Application.Abstract;
using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.Query;
using Microsoft.Extensions.DependencyInjection;

namespace AipsCore.Application.Common.Dispatcher;

public sealed class Dispatcher : IDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public Dispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    #region Execute

    public async Task Execute(ICommand command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
        
        await this.Handle(handlerType, command, cancellationToken);
    }

    public async Task<TResult> Execute<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
        
        return await this.HandleWithResult<TResult>(handlerType, command, cancellationToken);
    }

    public async Task<TResult> Execute<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        
        return await this.HandleWithResult<TResult>(handlerType, query, cancellationToken);
    }
    
    #endregion

    #region Handle
    
    private async Task Handle(Type handlerType, object commandOrQuery, CancellationToken cancellationToken = default)
    {
        dynamic handler = this.ResolveHandler(handlerType, commandOrQuery);
        
        await handler.Handle((dynamic)commandOrQuery, cancellationToken);
    }
    
    private async Task<TResult> HandleWithResult<TResult>(Type handlerType, object commandOrQuery, CancellationToken cancellationToken = default)
    {
        dynamic handler = this.ResolveHandler(handlerType, commandOrQuery);
        
        return await handler.Handle((dynamic)commandOrQuery, cancellationToken);
    }
    
    #endregion

    private dynamic ResolveHandler(Type handlerType, object commandOrQuery)
    {
        dynamic handler;

        try
        {
            handler = _serviceProvider.GetRequiredService(handlerType);
        }
        catch (InvalidOperationException serviceProviderException)
        {
            throw new DispatchException(commandOrQuery, serviceProviderException);
        }

        return handler;
    }
}