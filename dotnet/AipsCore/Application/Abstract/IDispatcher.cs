using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Abstract.Query;

namespace AipsCore.Application.Abstract;

public interface IDispatcher
{
    Task Execute(ICommand command, CancellationToken cancellationToken = default);
    Task<TCommandResult> Execute<TCommandResult>(ICommand<TCommandResult> command, CancellationToken cancellationToken = default)
        where TCommandResult : ICommandResult;
    
    Task<TQueryResult> Execute<TQueryResult>(IQuery<TQueryResult> query, CancellationToken cancellationToken = default)
        where TQueryResult : IQueryResult;

    public Task Execute(IMessage message, CancellationToken cancellationToken = default);
}