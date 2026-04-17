using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.Validation;

namespace AipsCore.Application.Abstract.Command;

public abstract class AbstractCommandHandlerStage<TCommand, TCommandHandlerContext> 
    : ICommandHandlerStage<TCommand, TCommandHandlerContext>
    where TCommand : ICommand
    where TCommandHandlerContext : ICommandHandlerContext
{
    protected abstract Task<TCommandHandlerContext> Execute(TCommand command, TCommandHandlerContext context, CancellationToken cancellationToken = default);

    public virtual ICollection<IRule> GetValidationRules(TCommand command, TCommandHandlerContext context)
    {
        return [];
    }

    public async Task<TCommandHandlerContext> Handle(TCommand command, TCommandHandlerContext context, CancellationToken cancellationToken = default)
    {
        var updatedContext = await Execute(command, context, cancellationToken);
        
        (this as IValidatable<TCommand, TCommandHandlerContext>).Validate(command, updatedContext);
        
        return updatedContext;
    }
}

public abstract class AbstractCommandHandlerStage<TCommand, TCommandResult, TCommandHandlerContext> 
    : ICommandHandlerStage<TCommand, TCommandResult, TCommandHandlerContext>
    where TCommand : ICommand<TCommandResult>
    where TCommandResult : ICommandResult
    where TCommandHandlerContext : ICommandHandlerContext<TCommandResult>
{
    protected abstract Task<TCommandHandlerContext> Prepare(TCommand command, TCommandHandlerContext context, CancellationToken cancellationToken = default);
    protected abstract Task<TCommandHandlerContext> Execute(TCommand command, TCommandHandlerContext context, CancellationToken cancellationToken = default);

    public virtual ICollection<IRule> GetValidationRules(TCommand command, TCommandHandlerContext context)
    {
        return [];
    }

    public async Task<TCommandHandlerContext> Handle(TCommand command, TCommandHandlerContext context, CancellationToken cancellationToken = default)
    {
        var updatedContext = await Prepare(command, context, cancellationToken);
        
        (this as IValidatable<TCommand, TCommandHandlerContext>).Validate(command, updatedContext);
        
        updatedContext = await Execute(command, updatedContext, cancellationToken);
        
        return updatedContext;
    }
}