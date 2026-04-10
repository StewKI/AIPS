using AipsCore.Application.Abstract.Command;

namespace AipsCore.Application.Models.Whiteboard.Command.JoinWithCode;

public sealed record JoinWithCodeCommand(string Code): ICommand<JoinWithCodeCommandResult>;