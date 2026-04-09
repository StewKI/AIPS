using AipsCore.Application.Abstract.Command;

namespace AipsCore.Application.Models.Whiteboard.Command.JoinWithCode;

public record JoinWithCodeCommand(string Code): ICommand<JoinWithCodeCommandResult>;