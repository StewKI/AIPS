namespace AipsCore.Application.Abstract.Command;

public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    Task Handle(TCommand command, CancellationToken cancellationToken = default);
}

public interface ICommandHandler<in TCommand, TCommandResult>
    where TCommand : ICommand<TCommandResult>
    where TCommandResult : ICommandResult
{
    Task<TCommandResult> Handle(TCommand command, CancellationToken cancellationToken = default);
}