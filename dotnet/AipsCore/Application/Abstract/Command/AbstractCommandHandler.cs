using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.Validation;

namespace AipsCore.Application.Abstract.Command;

public abstract class AbstractCommandHandler<TCommand, TCommandHandlerContext>
    : ICommandHandler<TCommand>, IValidatable<TCommand, TCommandHandlerContext>
    where TCommand : ICommand
    where TCommandHandlerContext : ICommandHandlerContext
{
    public async Task Handle(TCommand command, CancellationToken cancellationToken = default)
    {
        var context = await Prepare(command, cancellationToken);
        
        ((IValidatable<TCommand, TCommandHandlerContext>)this).Validate(command, context);
        
        await HandleInternal(command, context, cancellationToken);
    }

    public virtual ICollection<IRule> GetValidationRules(TCommand command, TCommandHandlerContext context)
    {
        return [];
    }
    
    protected abstract Task<TCommandHandlerContext> Prepare(TCommand command, CancellationToken cancellationToken = default);

    protected abstract Task HandleInternal(TCommand command, TCommandHandlerContext context, CancellationToken cancellationToken = default);
}

public abstract class AbstractCommandHandler<TCommand, TCommandResult, TCommandHandlerContext> 
    : ICommandHandler<TCommand, TCommandResult>, IValidatable<TCommand, TCommandHandlerContext>
    where TCommand : ICommand<TCommandResult>
    where TCommandResult : ICommandResult
    where TCommandHandlerContext : ICommandHandlerContext
{
    public async Task<TCommandResult> Handle(TCommand command, CancellationToken cancellationToken = default)
    {
        var context = await Prepare(command, cancellationToken);
        
        ((IValidatable<TCommand, TCommandHandlerContext>)this).Validate(command, context);
        
        return await HandleInternal(command, context, cancellationToken);
    }

    public virtual ICollection<IRule> GetValidationRules(TCommand command, TCommandHandlerContext context)
    {
        return [];
    }
    
    protected abstract Task<TCommandHandlerContext> Prepare(TCommand command, CancellationToken cancellationToken = default);

    protected abstract Task<TCommandResult> HandleInternal(TCommand command, TCommandHandlerContext context, CancellationToken cancellationToken = default);
}