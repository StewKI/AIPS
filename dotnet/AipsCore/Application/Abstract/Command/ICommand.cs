namespace AipsCore.Application.Abstract.Command;

public interface ICommand;

public interface ICommand<TCommandResult>
    where TCommandResult : ICommandResult;