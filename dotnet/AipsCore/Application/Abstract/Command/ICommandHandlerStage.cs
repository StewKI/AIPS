using AipsCore.Domain.Abstract.Validation;

namespace AipsCore.Application.Abstract.Command;

public interface ICommandHandlerStage<in TCommand, TCommandHandlerContext> 
    : IValidatable<TCommand, TCommandHandlerContext>
    where TCommand : ICommand
    where TCommandHandlerContext : ICommandHandlerContext
{
    Task<TCommandHandlerContext> Handle(TCommand command, TCommandHandlerContext context, CancellationToken cancellationToken = default);
}

public interface ICommandHandlerStage<in TCommand, TCommandResult, TCommandHandlerContext> 
    : IValidatable<TCommand, TCommandHandlerContext>
    where TCommand : ICommand<TCommandResult>
    where TCommandResult : ICommandResult
    where TCommandHandlerContext : ICommandHandlerContext<TCommandResult>
{
    Task<TCommandHandlerContext> Handle(TCommand command, TCommandHandlerContext context, CancellationToken cancellationToken = default);
}