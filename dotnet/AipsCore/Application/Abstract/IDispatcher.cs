using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.Query;

namespace AipsCore.Application.Abstract;

public interface IDispatcher
{
    Task Execute(ICommand command, CancellationToken cancellationToken = default);
    Task<TResult> Execute<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
    
    Task<TResult> Execute<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
}