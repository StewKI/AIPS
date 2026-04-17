namespace AipsCore.Application.Abstract.Command;

public abstract class AbstractStagedCommandHandler<TCommand, TCommandHandlerContext>
    : IStagedCommandHandler<TCommand, TCommandHandlerContext>
    where TCommand : ICommand
    where TCommandHandlerContext : ICommandHandlerContext
{
    public async Task Handle(TCommand command, CancellationToken cancellationToken = default)
    {
        var context = await InitializeContext(command, cancellationToken);
        
        foreach (var stage in GetCommandHandlerStages())
        {
            context = await stage.Handle(command, context, cancellationToken);
        }
    }

    protected abstract Task<TCommandHandlerContext> InitializeContext(TCommand command, CancellationToken cancellationToken = default);
    public abstract ICollection<ICommandHandlerStage<TCommand, TCommandHandlerContext>> GetCommandHandlerStages();
}

public abstract class AbstractStagedCommandHandler<TCommand, TCommandResult, TCommandHandlerContext>
    : IStagedCommandHandler<TCommand, TCommandResult, TCommandHandlerContext>
    where TCommand : ICommand<TCommandResult>
    where TCommandResult : ICommandResult
    where TCommandHandlerContext : ICommandHandlerContext<TCommandResult>
{
    public async Task<TCommandResult> Handle(TCommand command, CancellationToken cancellationToken = default)
    {
        var context = await InitializeContext(command, cancellationToken);
        
        foreach (var stage in GetCommandHandlerStages())
        {
            context = await stage.Handle(command, context, cancellationToken);

            if (context.Result is not null)
            {
                return context.Result;
            }
        }
        
        throw new InvalidOperationException("Command handler did not produce a result.");
    }

    protected abstract Task<TCommandHandlerContext> InitializeContext(TCommand command, CancellationToken cancellationToken = default);
    public abstract ICollection<ICommandHandlerStage<TCommand, TCommandResult, TCommandHandlerContext>> GetCommandHandlerStages();
}